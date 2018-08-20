using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using System.IO;
using System.IO.Compression;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net
{
    public class BittrexSocketClient: ExchangeClient, IBittrexSocketClient
    {
        #region fields
        private static BittrexSocketClientOptions defaultOptions = new BittrexSocketClientOptions();
        
        private const string HubName = "c2";

        private const string BalanceEvent = "uB";
        private const string MarketEvent = "uE";
        private const string SummaryLiteEvent = "uL";
        private const string SummaryEvent = "uS";
        private const string OrderEvent = "uO";

        private const string SummaryDeltaSub = "SubscribeToSummaryDeltas";
        private const string SummaryLiteDeltaSub = "SubscribeToSummaryLiteDeltas";
        private const string ExchangeDeltaSub = "SubscribeToExchangeDeltas";
        private const string QueryExchangeStateRequest = "QueryExchangeState";
        private const string QuerySummaryStateRequest = "QuerySummaryState";

        private IHubConnection connection;
        private IHubProxy proxy;

        private readonly List<BittrexRegistration> registrations = new List<BittrexRegistration>();
        private static int lastStreamId;

        private bool reconnecting;
        private bool authenticated;

        private static readonly object streamIdLock = new object();
        private readonly object connectionLock = new object();
        private readonly object registrationLock = new object();
        
        private static int NextStreamId
        {
            get
            {
                lock (streamIdLock)
                {
                    lastStreamId += 1;
                    return lastStreamId;
                }
            }
        }
        #endregion

        #region properties
        public IConnectionFactory ConnectionFactory { get; set; } = new ConnectionFactory();
        #endregion

        /// <summary>
        /// Connection to the server is lost
        /// </summary>
        public event Action ConnectionLost;
        /// <summary>
        /// Connection to the server is restored
        /// </summary>
        public event Action ConnectionRestored;
        /// <summary>
        /// Socket opened the connection to the bittrex server event
        /// </summary>
        public event Action Opened;
        /// <summary>
        /// Socket connection closed event
        /// </summary>
        public event Action Closed;
        /// <summary>
        /// Socket state changed event
        /// </summary>
        public event Action<StateChange> StateChanged;
        /// <summary>
        /// Socket error event. Note that this is only for errors thrown by the socket, not for errors in specific calls/events
        /// </summary>
        public event Action<Exception> Error;
        /// <summary>
        /// Socket connection slow event. Might indicate a lost connection
        /// </summary>
        public event Action Slow;
        #region ctor
        /// <summary>
        /// Creates a new socket client using the default options
        /// </summary>
        public BittrexSocketClient(): this(defaultOptions)
        {
        }

        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClient(BittrexSocketClientOptions options): base(options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            registrations = new List<BittrexRegistration>();

            Configure(options);
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Set the default options for new clients
        /// </summary>
        /// <param name="options">Options to use for new clients</param>
        public static void SetDefaultOptions(BittrexSocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Synchronized version of the <see cref="QuerySummaryStatesAsync"/> method
        /// </summary>
        public CallResult<List<BittrexStreamMarketSummary>> QuerySummaryStates() => QuerySummaryStatesAsync().Result;

        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        public async Task<CallResult<List<BittrexStreamMarketSummary>>> QuerySummaryStatesAsync()
        {
            if (!CheckConnection())
                return new CallResult<List<BittrexStreamMarketSummary>>(null, new CantConnectError());

            log.Write(LogVerbosity.Debug, "Querying summary states");
            var data = await InvokeProxy<string>(QuerySummaryStateRequest).ConfigureAwait(false);
            if (!data.Success)
                return new CallResult<List<BittrexStreamMarketSummary>>(null, data.Error);

            var result = await DecodeAndDeserializeData<BittrexStreamMarketSummariesQuery>(data.Data).ConfigureAwait(false);
            if (result.Success)
                return new CallResult<List<BittrexStreamMarketSummary>>(result.Data.Deltas, null);
            return new CallResult<List<BittrexStreamMarketSummary>>(null, result.Error);
        }

        /// <summary>
        /// Synchronized version of the <see cref="QueryExchangeStateAsync"/> method
        /// </summary>
        public CallResult<BittrexStreamQueryExchangeState> QueryExchangeState(string marketName) => QueryExchangeStateAsync(marketName).Result;

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="marketName">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        public async Task<CallResult<BittrexStreamQueryExchangeState>> QueryExchangeStateAsync(string marketName)
        {
            if (!CheckConnection())
                return new CallResult<BittrexStreamQueryExchangeState>(null, new CantConnectError());
            
            log.Write(LogVerbosity.Debug, "Querying exchange state for " + marketName);
            var data = await InvokeProxy<string>(QueryExchangeStateRequest, marketName).ConfigureAwait(false);
            if (!data.Success)
                return new CallResult<BittrexStreamQueryExchangeState>(null, data.Error);

            var result = await DecodeAndDeserializeData<BittrexStreamQueryExchangeState>(data.Data).ConfigureAwait(false);
            if(result.Success)
                result.Data.MarketName = marketName;
            return result;
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToExchangeStateUpdatesAsync"/> method
        /// </summary>
        public CallResult<int> SubscribeToExchangeStateUpdates(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate) => SubscribeToExchangeStateUpdatesAsync(marketName, onUpdate).Result;

        /// <summary>
        /// Subscribes to orderbook and trade updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToExchangeStateUpdatesAsync(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate)
        {
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            log.Write(LogVerbosity.Info, $"Subscribing to exchange state updates for {marketName}");
            var subResult = await InvokeProxy<bool>(ExchangeDeltaSub, marketName).ConfigureAwait(false);
            if (!subResult.Success || !subResult.Data)
                return new CallResult<int>(0, subResult.Error ?? new ServerError("Subscribe returned false"));

            var registration = new BittrexExchangeStateRegistration { Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId };
            lock (registrationLock)
                registrations.Add(registration);

            return new CallResult<int>(registration.StreamId, null);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToMarketSummariesUpdateAsync"/> method
        /// </summary>
        public CallResult<int> SubscribeToMarketSummariesUpdate(Action<List<BittrexStreamMarketSummary>> onUpdate) => SubscribeToMarketSummariesUpdateAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToMarketSummariesUpdateAsync(Action<List<BittrexStreamMarketSummary>> onUpdate)
        {
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            log.Write(LogVerbosity.Info, "Subscribing to market summaries updates");
            var subResult = await InvokeProxy<bool>(SummaryDeltaSub).ConfigureAwait(false);
            if (!subResult.Success || !subResult.Data)
                return new CallResult<int>(0, subResult.Error ?? new ServerError("Subscribe returned false"));

            var registration = new BittrexMarketSummariesRegistration { Callback = onUpdate, StreamId = NextStreamId };
            lock (registrationLock)            
                registrations.Add(registration);

            return new CallResult<int>(registration.StreamId, null);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToMarketSummariesLiteUpdateAsync"/> method
        /// </summary>
        public CallResult<int> SubscribeToMarketSummariesLiteUpdate(Action<List<BittrexStreamMarketSummaryLite>> onUpdate) => SubscribeToMarketSummariesLiteUpdateAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToMarketSummariesLiteUpdateAsync(Action<List<BittrexStreamMarketSummaryLite>> onUpdate)
        {
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            log.Write(LogVerbosity.Info, "Subscribing to market summaries lite updates");
            var subResult = await InvokeProxy<bool>(SummaryLiteDeltaSub).ConfigureAwait(false);
            if (!subResult.Success || !subResult.Data)
                return new CallResult<int>(0, subResult.Error ?? new ServerError("Subscribe returned false"));

            var registration = new BittrexMarketSummariesLiteRegistration { Callback = onUpdate, StreamId = NextStreamId };
            lock (registrationLock)
                registrations.Add(registration);

            return new CallResult<int>(registration.StreamId, null);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToBalanceUpdatesAsync"/> method
        /// </summary>
        public CallResult<int> SubscribeToBalanceUpdates(Action<BittrexStreamBalance> onUpdate) => SubscribeToBalanceUpdatesAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToBalanceUpdatesAsync(Action<BittrexStreamBalance> onUpdate)
        {
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            log.Write(LogVerbosity.Info, "Subscribing to balance updates");
            if (!authenticated)
            {
                var authResult = await Authenticate().ConfigureAwait(false); 
                if (!authResult.Success)
                    return new CallResult<int>(0, authResult.Error);
            }

            var registration = new BittrexBalanceUpdateRegistration { Callback = onUpdate, StreamId = NextStreamId };
            lock (registrationLock)
                registrations.Add(registration);

            return new CallResult<int>(registration.StreamId, null);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToOrderUpdatesAsync"/> method
        /// </summary>
        public CallResult<int> SubscribeToOrderUpdates(Action<BittrexStreamOrderData> onUpdate) => SubscribeToOrderUpdatesAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to order updates
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToOrderUpdatesAsync(Action<BittrexStreamOrderData> onUpdate)
        {
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            log.Write(LogVerbosity.Info, "Subscribing to order updates");
            if (!authenticated)
            {
                var authResult = await Authenticate().ConfigureAwait(false);
                if (!authResult.Success)
                    return new CallResult<int>(0, authResult.Error);
            }

            var registration = new BittrexOrderUpdateRegistration { Callback = onUpdate, StreamId = NextStreamId };
            lock (registrationLock)
                registrations.Add(registration);

            return new CallResult<int>(registration.StreamId, null);
        }

        /// <summary>
        /// Unsubsribe from updates of a specific stream using the stream id acquired when subscribing
        /// </summary>
        /// <param name="streamId">The stream id of the stream to unsubscribe</param>
        public void UnsubscribeFromStream(int streamId)
        {
            log.Write(LogVerbosity.Debug, $"Unsubscribing stream with id {streamId}");
            lock (registrationLock)
                registrations.RemoveAll(r => r.StreamId == streamId);
            
            CheckStop();
        }

        /// <summary>
        /// Unsubscribes all streams on this client
        /// </summary>
        public void UnsubscribeAllStreams()
        {
            log.Write(LogVerbosity.Info, "Unsubscribing all streams on this client");
            lock (registrationLock)
                registrations.Clear();

            Stop();
        }
        
        public override void Dispose()
        {
            log.Write(LogVerbosity.Debug, "Disposing socket client, unsubscribing all streams");

            base.Dispose();
            UnsubscribeAllStreams();
        }
        #endregion
        #region private
        private void Configure(BittrexSocketClientOptions options)
        {
            base.Configure(options);
        }

        private async Task<CallResult<T>> InvokeProxy<T>(string call, params string[] pars)
        {
            try
            {
                var sub = await proxy.Invoke<T>(call, pars).ConfigureAwait(false);
                return new CallResult<T>(sub, null);
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Warning, "Failed to invoke proxy: " + e.Message);
                return new CallResult<T>(default(T), new UnknownError("Failed to invoke proxy: " + e.Message));
            }
        }

        private async Task<CallResult<bool>> Authenticate()
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());
            
            log.Write(LogVerbosity.Debug, "Starting authentication");
            var result = await InvokeProxy<string>("GetAuthContext", authProvider.Credentials.Key.GetString()).ConfigureAwait(false);
            if (!result.Success)
            {
                log.Write(LogVerbosity.Error, "Authentication failed, api key is probably invalid");
                return new CallResult<bool>(false, result.Error);
            }

            log.Write(LogVerbosity.Debug, "Auth context retrieved");
            var signed = authProvider.Sign(result.Data);
            var authResult = await InvokeProxy<bool>("Authenticate", authProvider.Credentials.Key.GetString(), signed).ConfigureAwait(false);
            if (!authResult.Success || !authResult.Data)
            {
                log.Write(LogVerbosity.Error, "Authentication failed, api secret is probably invalid");
                return new CallResult<bool>(false, authResult.Error ?? new ServerError("Authentication failed"));
            }

            authenticated = true;
            log.Write(LogVerbosity.Info, "Authentication successful");
            return new CallResult<bool>(true, null);
        }

        private async Task<CallResult<T>> DecodeAndDeserializeData<T>(string rawData) where T : class
        {
            try
            {
                byte[] gzipData = Convert.FromBase64String(rawData);
                using (var decompressedStream = new MemoryStream())
                using (var compressedStream = new MemoryStream(gzipData))
                using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(decompressedStream);
                    decompressedStream.Position = 0;

                    using (var streamReader = new StreamReader(decompressedStream))
                    {
                        var data = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                        if (data == "null")
                            return new CallResult<T>(null, new DeserializeError("Server returned null"));

                        return Deserialize<T>(data);
                    }
                }
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Info, "Exception in decode data: " + e.Message);
                return new CallResult<T>(null, new DeserializeError("Exception in decode data: " + e.Message));
            }
        }
        
        private void CheckStop()
        {
            bool shouldStop;
            lock (registrationLock)
                shouldStop = !registrations.Any();

            if (shouldStop)
                Stop();
        }

        private void Stop()
        {
            Task.Run(() =>
            {
                lock (connectionLock)
                {
                    log.Write(LogVerbosity.Info, "No more subscriptions, stopping the socket");
                    connection.Stop(TimeSpan.FromSeconds(1));
                }
            });
        }

        private bool CheckConnection()
        {
            lock (connectionLock)
            {
                if (connection != null && connection.State != ConnectionState.Disconnected)
                    return true;

                log.Write(LogVerbosity.Info, "Starting connection to bittrex server");
                return WaitForConnection();
            }
        }

        private bool WaitForConnection()
        {
            lock (connectionLock)
            {
                if (connection == null)
                {
                    connection = ConnectionFactory.Create(log, baseAddress);
                    if (apiProxy != null)
                        connection.SetProxy(apiProxy.Host, apiProxy.Port);
                    
                    proxy = connection.CreateHubProxy(HubName);

                    proxy.On(MarketEvent, (data) => SocketMessageExchangeState(data));
                    proxy.On(SummaryEvent, (data) => SocketMessageMarketSummaries(data));
                    proxy.On(SummaryLiteEvent, (data) => SocketMessageMarketSummariesLite(data));
                    proxy.On(BalanceEvent, (data) => SocketMessageBalance(data));
                    proxy.On(OrderEvent, (data) => SocketMessageOrder(data));

                    connection.Closed += SocketClosed;
                    connection.Error += SocketError;
                    connection.ConnectionSlow += SocketSlow;
                    connection.StateChanged += SocketStateChange;                    
                }

                // Try connecting
                var connectResult = TryStart().ConfigureAwait(false).GetAwaiter().GetResult();

                if (!connectResult)
                    return connectResult;

                // Resubscribe the subscriptions
                List<BittrexRegistration> registrationsCopy;
                lock (registrationLock)
                    registrationsCopy = registrations.ToList();

                if(registrationsCopy.Count > 0)
                    log.Write(LogVerbosity.Info, $"Resubscribing {registrationsCopy.Count} subscriptions");

                bool failedResubscribe = false;
                foreach (var registration in registrationsCopy)
                {
                    if (registration is BittrexMarketSummariesRegistration)
                    {
                        var resubSuccess = InvokeProxy<bool>(SummaryDeltaSub).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!resubSuccess.Success)
                        {
                            log.Write(LogVerbosity.Warning, "Failed to resubscribe summary delta: " + resubSuccess.Error);
                            failedResubscribe = true;
                            break;
                        }
                    }
                    else if (registration is BittrexExchangeStateRegistration)
                    {
                        var resubSuccess = InvokeProxy<bool>(ExchangeDeltaSub, ((BittrexExchangeStateRegistration)registration).MarketName).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!resubSuccess.Success)
                        {
                            log.Write(LogVerbosity.Warning, "Failed to resubscribe exchange delta: " + resubSuccess.Error);
                            failedResubscribe = true;
                            break;
                        }
                    }
                    else if (registration is BittrexMarketSummariesLiteRegistration)
                    {
                        var resubSuccess = InvokeProxy<bool>(SummaryLiteDeltaSub).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!resubSuccess.Success)
                        {
                            log.Write(LogVerbosity.Warning, "Failed to resubscribe summary lite delta: " + resubSuccess.Error);
                            failedResubscribe = true;
                            break;
                        }
                    }
                    else if (registration is BittrexBalanceUpdateRegistration || registration is BittrexOrderUpdateRegistration)
                    {
                        if (!authenticated)
                        {
                            var authResult = Authenticate().ConfigureAwait(false).GetAwaiter().GetResult();
                            if (!authResult.Success)
                            {
                                log.Write(LogVerbosity.Warning, "Failed to re-authenticate: " + authResult.Error);
                                failedResubscribe = true;
                                break;
                            }
                        }
                    }
                }

                if (failedResubscribe)
                {
                    log.Write(LogVerbosity.Warning, "Failed to resubscribe all running subscriptions -> Reconnect and try again");
                    connection.Stop(TimeSpan.FromSeconds(1));
                    return false;
                }

                return connectResult;
            }
        }

        private async Task<bool> TryStart()
        {
            try
            {
                await connection.Start().ConfigureAwait(false);
                Opened?.Invoke();
            }
            catch (Exception ex)
            {
                log.Write(LogVerbosity.Warning, $"Couldn't connect. {ex.GetType()}");
                log.Write(LogVerbosity.Debug, $" {ex.Message}");
                return false;
            }
            
            log.Write(LogVerbosity.Info, "Socket connection established");            
            return true;
        }
        
        private void SocketMessageExchangeState(string data)
        {
            var dataResult = DecodeAndDeserializeData<BittrexStreamUpdateExchangeState>(data).Result;
            if(!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Failed to decode and deserialize ExchangeState data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexExchangeStateRegistration> marketRegistrations;
            lock (registrationLock)
                marketRegistrations = registrations.OfType<BittrexExchangeStateRegistration>().Where(x => x.MarketName == dataResult.Data.MarketName).ToList();

            Parallel.ForEach(marketRegistrations, registration => registration.Callback(dataResult.Data));
        }

        private void SocketMessageMarketSummaries(string data)
        {
            var dataResult = DecodeAndDeserializeData<BittrexStreamMarketSummaryUpdate>(data).Result;
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Failed to decode and deserialize MarketSummaries data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexMarketSummariesRegistration> listeners;
            lock (registrationLock)
                listeners = registrations.OfType<BittrexMarketSummariesRegistration>().ToList();

            Parallel.ForEach(listeners, allRegistration => allRegistration.Callback(dataResult.Data.Deltas));
        }

        private void SocketMessageMarketSummariesLite(string data)
        {
            var dataResult = DecodeAndDeserializeData<BittrexStreamMarketSummariesLite>(data).Result;
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Failed to decode and deserialize MarketSummariesLite data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexMarketSummariesLiteRegistration> listeners;
            lock (registrationLock)
                listeners = registrations.OfType<BittrexMarketSummariesLiteRegistration>().ToList();

            Parallel.ForEach(listeners, allRegistration => allRegistration.Callback(dataResult.Data.Deltas));
        }

        private void SocketMessageBalance(string data)
        {
            var dataResult = DecodeAndDeserializeData<BittrexStreamBalanceData>(data).Result;
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Failed to decode and deserialize Balance data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexBalanceUpdateRegistration> listeners;
            lock (registrationLock)
                listeners = registrations.OfType<BittrexBalanceUpdateRegistration>().ToList();

            Parallel.ForEach(listeners, allRegistration => allRegistration.Callback(dataResult.Data.Delta));
        }

        private void SocketMessageOrder(string data)
        {
            var dataResult = DecodeAndDeserializeData<BittrexStreamOrderData>(data).Result;
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Failed to decode and deserialize Order data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexOrderUpdateRegistration> listeners;
            lock (registrationLock)
                listeners = registrations.OfType<BittrexOrderUpdateRegistration>().ToList();

            Parallel.ForEach(listeners, allRegistration => allRegistration.Callback(dataResult.Data));
        }

        private void SocketStateChange(StateChange state)
        {
            log.Write(LogVerbosity.Debug, $"Socket state: {state.OldState} -> {state.NewState}");
            StateChanged?.Invoke(state);
        }

        private void SocketSlow()
        {
            log.Write(LogVerbosity.Warning, "Socket connection slow");
            Slow?.Invoke();
        }

        private void SocketError(Exception exception)
        {
            log.Write(LogVerbosity.Error, $"Socket error: {exception.Message}");
            Error?.Invoke(exception);
        }

        private void SocketClosed()
        {
            log.Write(LogVerbosity.Info, "Socket closed");
            Closed?.Invoke();
            authenticated = false;
            var shouldReconnect = false;

            if (!reconnecting)
            {
                lock (registrationLock)
                    if (registrations.Any())
                        shouldReconnect = true;
            }

            if (!shouldReconnect)
                return;

            reconnecting = true;
            ConnectionLost?.Invoke();
            Task.Run(() => TryReconnect());
        }

        private void TryReconnect()
        {
            bool shouldTry;
            lock (registrationLock)
                shouldTry = registrations.Any();

            if (shouldTry)
            {
                if (!WaitForConnection())
                {
                    Thread.Sleep(5000);
                    Task.Run(() => TryReconnect());
                }
                else
                {
                    log.Write(LogVerbosity.Info, "Reconnected");
                    reconnecting = false;
                    ConnectionRestored?.Invoke();
                }
            }
            else
                reconnecting = false;
        }
#endregion
#endregion
    }
}
