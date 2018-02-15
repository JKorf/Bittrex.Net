using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Errors;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Bittrex.Net.Sockets;

namespace Bittrex.Net
{
    public class BittrexSocketClient: BittrexAbstractClient
    {
        #region fields
        private const string BaseAddress = "https://www.bittrex.com/";
        private const string SocketAddress = "https://socket.bittrex.com/";

        private const string HubName = "coreHub";
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36 OPR/48.0.2685.52";

        private const string MarketDeltaEvent = "updateSummaryState";
        private const string ExchangeStateEvent = "updateExchangeState";
        private const string MarketDeltaSub = "SubscribeToSummaryDeltas";

        private static Interfaces.IHubConnection connection;
        private static IHubProxy proxy;

        private readonly List<BittrexRegistration> localRegistrations;
        private static readonly List<BittrexRegistration> registrations = new List<BittrexRegistration>();
        private static int lastStreamId;

        private static bool reconnecting;
        private string proxyHost;
        private int proxyPort;

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
        public IConnectionFactory ConnectionFactory = new ConnectionFactory();
        public ICloudFlareAuthenticator CloudFlareAuthenticator = new CloudFlareAuthenticator();
        public int CloudFlareRetries { get; set; } = 2;
        #endregion

        public static event Action ConnectionLost;
        public static event Action ConnectionRestored;



        #region ctor
        public BittrexSocketClient()
        {
            localRegistrations = new List<BittrexRegistration>();
        }
        #endregion

        #region methods
        #region public

        /// <summary>
        /// Set Proxy for Websocket Communication (only use DNS)
        /// </summary>
        /// <param name="hostName">The proxy host name</param>
        /// <param name="port">The proxy port</param>
        public void SetProxy(String hostName, int port)
        {
            proxyHost = hostName;
            proxyPort = port;
        }

        /// <summary>
        /// Synchronized version of the <see cref="QueryExchangeStateAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexExchangeState> QueryExchangeState(string marketName) => QueryExchangeStateAsync(marketName).Result;

        /// <summary>
        /// Gets basic/initial info of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <returns>The current exchange state</returns>
        public async Task<BittrexApiResult<BittrexExchangeState>> QueryExchangeStateAsync(string marketName)
        {
            if (!CheckConnection())
                return ThrowErrorMessage<BittrexExchangeState>(BittrexErrors.GetError(BittrexErrorKey.CantConnectToServer));
            
            var result = await proxy.Invoke<JObject>("QueryExchangeState", marketName);

            string json = null;
            try
            {
                json = result.ToString();
                BittrexExchangeState streamData = JsonConvert.DeserializeObject<BittrexExchangeState>(json);
                streamData.MarketName = marketName;
                return new BittrexApiResult<BittrexExchangeState>() {Success = true, Result = streamData};
            }
            catch (Exception e)
            {
                var data = $"Received an event but an unknown error occured. Message: {e.Message}, Received data: {json}";
                log.Write(LogVerbosity.Warning, data);
                return ThrowErrorMessage<BittrexExchangeState>(BittrexErrors.GetError(BittrexErrorKey.UnknownError), data);
            }
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToMarketDeltaStreamAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<int> SubscribeToMarketDeltaStream(string marketName, Action<BittrexMarketSummary> onUpdate) => SubscribeToMarketDeltaStreamAsync(marketName, onUpdate).Result;

        /// <summary>
        /// Subscribes to filled orders on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<BittrexApiResult<int>> SubscribeToMarketDeltaStreamAsync(string marketName, Action<BittrexMarketSummary> onUpdate)
        {
            return await Task.Run(() =>
            {
                log.Write(LogVerbosity.Debug, $"Going to subscribe to {marketName}");
                if (!CheckConnection())
                    return ThrowErrorMessage<int>(BittrexErrors.GetError(BittrexErrorKey.CantConnectToServer));

                var registration = new BittrexMarketsRegistration() { Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId };
                lock (registrationLock)
                {
                    registrations.Add(registration);
                    localRegistrations.Add(registration);
                }
                return new BittrexApiResult<int>() { Result = registration.StreamId, Success = true };
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToExchangeDeltasAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<int> SubscribeToExchangeDeltas(string marketName, Action<BittrexStreamExchangeState> onUpdate) => SubscribeToExchangeDeltasAsync(marketName, onUpdate).Result;

        /// <summary>
        /// Subscribes to updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<BittrexApiResult<int>> SubscribeToExchangeDeltasAsync(string marketName, Action<BittrexStreamExchangeState> onUpdate)
        {
            if (!CheckConnection())
                return ThrowErrorMessage<int>(BittrexErrors.GetError(BittrexErrorKey.CantConnectToServer));

            // send subscribe to bittrex
            await SubscribeToExchangeDeltas(marketName);

            var registration = new BittrexExchangeDeltasRegistration() { Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId };
            lock (registrationLock)
            {
                registrations.Add(registration);
                localRegistrations.Add(registration);
            }
            return new BittrexApiResult<int>() { Result = registration.StreamId, Success = true };
        }

        private async Task SubscribeToExchangeDeltas(string marketName)
        {
            log.Write(LogVerbosity.Debug, $"Going to subscribe to ExchangeDeltas of {marketName}");

            // when subscribing this we get Exchange State method calls regularly
            await proxy.Invoke("SubscribeToExchangeDeltas", marketName);
        }

        /// <summary>
        /// Synchronized version of the <see cref="SubscribeToAllMarketDeltaStreamAsync"/> method
        /// </summary>
        /// <param name="onUpdate"></param>
        /// <returns></returns>
        public BittrexApiResult<int> SubscribeToAllMarketDeltaStream(Action<List<BittrexMarketSummary>> onUpdate) => SubscribeToAllMarketDeltaStreamAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to updates of all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        public async Task<BittrexApiResult<int>> SubscribeToAllMarketDeltaStreamAsync(Action<List<BittrexMarketSummary>> onUpdate)
        {
            return await Task.Run(() =>
            {
                log.Write(LogVerbosity.Debug, $"Going to subscribe to all markets");
                if (!CheckConnection())
                    return ThrowErrorMessage<int>(BittrexErrors.GetError(BittrexErrorKey.CantConnectToServer));

                var registration = new BittrexMarketsAllRegistration() { Callback = onUpdate, StreamId = NextStreamId };
                lock (registrationLock)
                {
                    registrations.Add(registration);
                    localRegistrations.Add(registration);
                }
                return new BittrexApiResult<int>() { Result = registration.StreamId, Success = true };
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
            log.Write(LogVerbosity.Debug, "Unsubscribing all streams on this client");
            lock (registrationLock)
            {
                registrations.RemoveAll(r => localRegistrations.Contains(r));
                localRegistrations.Clear();
            }

            CheckStop();
        }
        
        public override void Dispose()
        {
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
                        log.Write(LogVerbosity.Debug, "No more subscriptions, stopping the socket");
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
                    log.Write(LogVerbosity.Debug, "Starting connection to bittrex server");
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
                    if (proxyHost != null && proxyPort != 0)
                        connection = ConnectionFactory.Create(SocketAddress, proxyHost, proxyPort);
                    else
                        connection = ConnectionFactory.Create(SocketAddress);
                    
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
                if (TryStart().Result)
                    return true;

                // If failed, try to get CloudFlare bypass
                log.Write(LogVerbosity.Warning, "Couldn't connect to Bittrex server, going to try CloudFlare bypass");
                var cookieContainer = CloudFlareAuthenticator.GetCloudFlareCookies(BaseAddress, UserAgent, CloudFlareRetries);
                if (cookieContainer == null)
                {
                    log.Write(LogVerbosity.Error, $"CloudFlareAuthenticator didn't give us the cookies");
                    return false;
                }

                connection.Cookies = cookieContainer;
                connection.UserAgent = UserAgent;
                log.Write(LogVerbosity.Debug, "CloudFlare cookies retrieved, retrying connection");

                // Try again with cookies
                return TryStart().Result;
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
                connection.Start().Wait();
            }
            catch (Exception ex)
            {
                log.Write(LogVerbosity.Debug, ex.ToString());
            }
            waitEvent.WaitOne();
            connection.StateChanged -= waitDelegate;
            
            if (connection.State == ConnectionState.Connected)
            {
                // subscribe to all market deltas
                await proxy.Invoke(MarketDeltaSub);
                
                IEnumerable<BittrexExchangeDeltasRegistration> marketRegistrations;
                lock (registrationLock)
                    marketRegistrations = registrations.OfType<BittrexExchangeDeltasRegistration>();

                foreach (var registration in marketRegistrations)
                    await SubscribeToExchangeDeltas(registration.MarketName);

                return true;
            }
            return false;
        }
        
        private void SocketMessageExchangeState(IList<JToken> jsonData)
        {
            if (jsonData.Count == 0 || jsonData[0] == null)
                return;

            BittrexStreamExchangeState data;
            try
            {
                data = JsonConvert.DeserializeObject<BittrexStreamExchangeState>(jsonData[0].ToString());
                if (data == null)
                    return;
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Warning, $"Received an event but an unknown error occured. Message: {e.Message}, Received data: {jsonData[0]}");
                return;
            }

            IEnumerable<BittrexExchangeDeltasRegistration> marketRegistrations;
            lock (registrationLock)
                marketRegistrations = registrations.OfType<BittrexExchangeDeltasRegistration>();

            Parallel.ForEach(marketRegistrations, registration =>
            {
                registration.Callback(data);
            });
        }

        private void SocketMessageMarketDeltas(IList<JToken> jsonData)
        {
            if (jsonData.Count == 0 || jsonData[0] == null)
                return;

            BittrexStreamDeltas data;
            try
            {
                data = JsonConvert.DeserializeObject<BittrexStreamDeltas>(jsonData[0].ToString());
                if (data == null)
                    return;
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Warning, $"Received an event but an unknown error occured. Message: {e.Message}, Received data: {jsonData[0]}");
                return;
            }

            IEnumerable<BittrexMarketsAllRegistration> allRegistrations;
            IEnumerable<BittrexMarketsRegistration> marketRegistrations;
            lock (registrationLock)
            {
                allRegistrations = registrations.OfType<BittrexMarketsAllRegistration>();
                marketRegistrations = registrations.OfType<BittrexMarketsRegistration>();
            }

            Parallel.ForEach(allRegistrations, allRegistration =>
            {
                allRegistration.Callback(data.Deltas);
            });

            Parallel.ForEach(data.Deltas, delta =>
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
            log.Write(LogVerbosity.Debug, "Socket closed");
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
            bool shouldTry = false;
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
