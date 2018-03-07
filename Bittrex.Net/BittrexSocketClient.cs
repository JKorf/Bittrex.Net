using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;

namespace Bittrex.Net
{
    public class BittrexSocketClient: ExchangeClient
    {
        #region fields
        private static BittrexSocketClientOptions defaultOptions = new BittrexSocketClientOptions();

        private string cloudFlareAuthenticationAddress;
        private string socketAddress;
        private int cloudFlareRetries;

        private const string HubName = "coreHub";
        internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36 OPR/48.0.2685.52";

        private const string MarketDeltaEvent = "updateSummaryState";
        private const string ExchangeStateEvent = "updateExchangeState";
        private const string MarketDeltaSub = "SubscribeToSummaryDeltas";

        private static Interfaces.IHubConnection connection;
        private static IHubProxy proxy;

        private readonly List<BittrexRegistration> localRegistrations;
        private static readonly List<BittrexRegistration> registrations = new List<BittrexRegistration>();
        private static int lastStreamId;

        private static bool reconnecting;

        private static readonly object streamIdLock = new object();
        private static readonly object connectionLock = new object();
        private static readonly object registrationLock = new object();
        
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
        public ICloudFlareAuthenticator CloudFlareAuthenticator { get; set; } = new CloudFlareAuthenticator();
        #endregion

        /// <summary>
        /// Event that gets called when the socket connection was lost
        /// </summary>
        public static event Action ConnectionLost;
        /// <summary>
        /// Event that gets called when the socket connection was restored
        /// </summary>
        public static event Action ConnectionRestored;
        
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
        public BittrexSocketClient(BittrexSocketClientOptions options): base(options, null)
        {
            localRegistrations = new List<BittrexRegistration>();

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

        private void Configure(BittrexSocketClientOptions options)
        {
            base.Configure(options);

            cloudFlareRetries = options.CloudFlareBypassRetries;
            cloudFlareAuthenticationAddress = options.CloudFlareAuthenticationAddress;
            socketAddress = options.SocketAddress;
        }

        /// <summary>
        /// Synchronized version of the <see cref="QueryExchangeStateAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexStreamQueryExchangeState> QueryExchangeState(string marketName) => QueryExchangeStateAsync(marketName).Result;

        /// <summary>
        /// Gets basic/initial info of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <returns>The current exchange state</returns>
        public async Task<CallResult<BittrexStreamQueryExchangeState>> QueryExchangeStateAsync(string marketName)
        {
            if (!CheckConnection())
                return new CallResult<BittrexStreamQueryExchangeState>(null, new CantConnectError());
            
            var result = await proxy.Invoke<JObject>("QueryExchangeState", marketName).ConfigureAwait(false);
            
            var dataResult = Deserialize<BittrexStreamQueryExchangeState>(result.ToString());
            if (!dataResult.Success)
                return dataResult;

            dataResult.Data.MarketName = marketName;
            return dataResult;
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToMarketDeltaStreamAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<int> SubscribeToMarketDeltaStream(string marketName, Action<BittrexMarketSummary> onUpdate) => SubscribeToMarketDeltaStreamAsync(marketName, onUpdate).Result;

        /// <summary>
        /// Subscribes to filled orders on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToMarketDeltaStreamAsync(string marketName, Action<BittrexMarketSummary> onUpdate)
        {
            return await Task.Run(() =>
            {
                log.Write(LogVerbosity.Info, $"Subscribing to market deltas for {marketName}");
                if (!CheckConnection())
                    return new CallResult<int>(0, new CantConnectError());

                var registration = new BittrexMarketsRegistration() { Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId };
                lock (registrationLock)
                {
                    registrations.Add(registration);
                    localRegistrations.Add(registration);
                }
                return new CallResult<int>(registration.StreamId, null);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToExchangeDeltasAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<int> SubscribeToExchangeDeltas(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate) => SubscribeToExchangeDeltasAsync(marketName, onUpdate).Result;

        /// <summary>
        /// Subscribes to updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToExchangeDeltasAsync(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate)
        {
            log.Write(LogVerbosity.Info, $"Subscribing to exchange deltas for {marketName}");
            if (!CheckConnection())
                return new CallResult<int>(0, new CantConnectError());

            // send subscribe to bittrex
            await SubscribeToExchangeDeltas(marketName).ConfigureAwait(false);

            var registration = new BittrexExchangeDeltasRegistration() { Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId };
            lock (registrationLock)
            {
                registrations.Add(registration);
                localRegistrations.Add(registration);
            }
            return new CallResult<int>(registration.StreamId, null);
        }

        private async Task SubscribeToExchangeDeltas(string marketName)
        {
            // when subscribing this we get Exchange State method calls regularly
            await proxy.Invoke("SubscribeToExchangeDeltas", marketName).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToAllMarketDeltaStreamAsync"/> method
        /// </summary>
        /// <param name="onUpdate"></param>
        /// <returns></returns>
        public CallResult<int> SubscribeToAllMarketDeltaStream(Action<List<BittrexMarketSummary>> onUpdate) => SubscribeToAllMarketDeltaStreamAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to updates of all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<CallResult<int>> SubscribeToAllMarketDeltaStreamAsync(Action<List<BittrexMarketSummary>> onUpdate)
        {
            return await Task.Run(() =>
            {
                log.Write(LogVerbosity.Info, "Subscribing to market deltas for all markets");
                if (!CheckConnection())
                    return new CallResult<int>(0, new CantConnectError());

                var registration = new BittrexMarketsAllRegistration() { Callback = onUpdate, StreamId = NextStreamId };
                lock (registrationLock)
                {
                    registrations.Add(registration);
                    localRegistrations.Add(registration);
                }
                return new CallResult<int>(registration.StreamId, null);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Unsubsribe from updates of a specific stream using the stream id acquired when subscribing
        /// </summary>
        /// <param name="streamId">The stream id of the stream to unsubscribe</param>
        public void UnsubscribeFromStream(int streamId)
        {
            log.Write(LogVerbosity.Debug, $"Unsubscribing stream with id {streamId}");
            lock (registrationLock)
            {
                localRegistrations.RemoveAll(r => r.StreamId == streamId);
                registrations.RemoveAll(r => r.StreamId == streamId);
            }
            
            CheckStop();
        }

        /// <summary>
        /// Unsubscribes all streams on this client
        /// </summary>
        public void UnsubscribeAllStreams()
        {
            log.Write(LogVerbosity.Info, "Unsubscribing all streams on this client");
            lock (registrationLock)
            {
                registrations.RemoveAll(r => localRegistrations.Contains(r));
                localRegistrations.Clear();
            }

            CheckStop();
        }
        
        public override void Dispose()
        {
            log.Write(LogVerbosity.Debug, "Disposing socket client, unsubscribing all streams");

            base.Dispose();
            UnsubscribeAllStreams();
        }
        #endregion
        #region private
        private void CheckStop()
        {
            bool shouldStop;
            lock (registrationLock)
                shouldStop = !registrations.Any();

            if (shouldStop)
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
        }

        private bool CheckConnection()
        {
            lock (connectionLock)
            {
                if (connection == null || connection.State == ConnectionState.Disconnected)
                {
                    log.Write(LogVerbosity.Info, "Starting connection to bittrex server");
                    return WaitForConnection();
                }
            }
            return true;
        }

        private bool WaitForConnection()
        {
            lock (connectionLock)
            {
                if (connection == null)
                {
                    connection = ConnectionFactory.Create(socketAddress);
                    if (apiProxy != null)
                        connection.SetProxy(apiProxy.Host, apiProxy.Port);
                    
                    proxy = connection.CreateHubProxy(HubName);

                    connection.Closed += SocketClosed;
                    connection.Error += SocketError;
                    connection.ConnectionSlow += SocketSlow;
                    connection.StateChanged += SocketStateChange;
                    
                    Subscription sub = proxy.Subscribe(MarketDeltaEvent);
                    sub.Received += SocketMessageMarketDeltas;

                    // regular updates
                    Subscription subExchangeState = proxy.Subscribe(ExchangeStateEvent);
                    subExchangeState.Received += SocketMessageExchangeState;
                }

                // Try to start
                if(TryStart().ConfigureAwait(false).GetAwaiter().GetResult())
                    return true;

                // If failed, try to get CloudFlare bypass
                log.Write(LogVerbosity.Warning, "Couldn't connect to Bittrex server, going to try CloudFlare bypass");
                var cookieContainer = CloudFlareAuthenticator.GetCloudFlareCookies(cloudFlareAuthenticationAddress, UserAgent, cloudFlareRetries);
                if (cookieContainer == null)
                {
                    log.Write(LogVerbosity.Error, "CloudFlareAuthenticator didn't give us the cookies");
                    return false;
                }

                connection.Cookies = cookieContainer;
                connection.UserAgent = UserAgent;
                log.Write(LogVerbosity.Info, "CloudFlare cookies retrieved, retrying connection");

                // Try again with cookies
                return TryStart().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private async Task<bool> TryStart()
        {
            var waitEvent = new ManualResetEvent(false);
            var waitDelegate = new Action<StateChange>((state) =>
            {
                if (state.NewState == ConnectionState.Connected ||
                    (state.NewState == ConnectionState.Disconnected &&
                     state.OldState == ConnectionState.Connecting))
                    waitEvent.Set();
            });

            connection.StateChanged += waitDelegate;
            try
            {
                await connection.Start().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Write(LogVerbosity.Warning, ex.ToString());
            }
            waitEvent.WaitOne();
            connection.StateChanged -= waitDelegate;
            
            if (connection.State == ConnectionState.Connected)
            {
                log.Write(LogVerbosity.Info, "Socket connection established");

                // subscribe to all market deltas
                await proxy.Invoke(MarketDeltaSub).ConfigureAwait(false);
                
                IEnumerable<BittrexExchangeDeltasRegistration> marketRegistrations;
                lock (registrationLock)
                    marketRegistrations = registrations.OfType<BittrexExchangeDeltasRegistration>();

                foreach (var registration in marketRegistrations)
                    await SubscribeToExchangeDeltas(registration.MarketName).ConfigureAwait(false);

                return true;
            }
            return false;
        }
        
        private void SocketMessageExchangeState(IList<JToken> jsonData)
        {
            if (jsonData.Count == 0 || jsonData[0] == null)
                return;

            var dataResult = Deserialize<BittrexStreamUpdateExchangeState>(jsonData[0].ToString(), false);
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Error deserializing ExchangeState data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexExchangeDeltasRegistration> marketRegistrations;
            lock (registrationLock)
                marketRegistrations = registrations.OfType<BittrexExchangeDeltasRegistration>().Where(x => x.MarketName == dataResult.Data.MarketName);

            Parallel.ForEach(marketRegistrations, registration => registration.Callback(dataResult.Data));
        }

        private void SocketMessageMarketDeltas(IList<JToken> jsonData)
        {
            if (jsonData.Count == 0 || jsonData[0] == null)
                return;

            var dataResult = Deserialize<BittrexStreamDeltas>(jsonData[0].ToString(), false);
            if (!dataResult.Success)
            {
                log.Write(LogVerbosity.Warning, "Error deserializing MarketDelta data: " + dataResult.Error);
                return;
            }

            IEnumerable<BittrexMarketsAllRegistration> allRegistrations;
            IEnumerable<BittrexMarketsRegistration> marketRegistrations;
            lock (registrationLock)
            {
                allRegistrations = registrations.OfType<BittrexMarketsAllRegistration>();
                marketRegistrations = registrations.OfType<BittrexMarketsRegistration>();
            }

            Parallel.ForEach(allRegistrations, allRegistration => allRegistration.Callback(dataResult.Data.Deltas));
            Parallel.ForEach(dataResult.Data.Deltas, delta =>
            {
                foreach (var update in marketRegistrations.Where(r => r.MarketName == delta.MarketName))
                    update.Callback(delta);
            });
        }

        private void SocketStateChange(StateChange state)
        {
            log.Write(LogVerbosity.Debug, $"Socket state: {state.OldState} -> {state.NewState}");
        }

        private void SocketSlow()
        {
            log.Write(LogVerbosity.Warning, "Socket connection slow");
        }

        private void SocketError(Exception exception)
        {
            log.Write(LogVerbosity.Error, $"Socket error: {exception.Message}");
        }

        private void SocketClosed()
        {
            log.Write(LogVerbosity.Info, "Socket closed");
            bool shouldReconnect = false;

            if (!reconnecting)
            {
                lock (registrationLock)
                    if (registrations.Any())
                        shouldReconnect = true;
            }

            if (shouldReconnect)
            {
                reconnecting = true;
                ConnectionLost?.Invoke();
                Task.Run(() => TryReconnect());
            }
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
                    TryReconnect();
                }
                else
                {
                    reconnecting = false;
                    ConnectionRestored?.Invoke();
                }
            }
            reconnecting = false;
        }
#endregion
#endregion
    }
}
