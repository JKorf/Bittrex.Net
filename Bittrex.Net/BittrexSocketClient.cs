using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex socket API
    /// </summary>
    public class BittrexSocketClient: SocketClient, IBittrexSocketClient
    {
        #region fields
        private static BittrexSocketClientOptions defaultOptions = new BittrexSocketClientOptions();
        private static BittrexSocketClientOptions DefaultOptions => defaultOptions.Copy<BittrexSocketClientOptions>();

        private const string HubName = "c2";
        
        private const string SummaryDeltaSub = "SubscribeToSummaryDeltas";
        private const string SummaryLiteDeltaSub = "SubscribeToSummaryLiteDeltas";
        private const string ExchangeDeltaSub = "SubscribeToExchangeDeltas";
        private const string QueryExchangeStateRequest = "QueryExchangeState";
        private const string QuerySummaryStateRequest = "QuerySummaryState";

        private const string ExchangeStateUpdate = "uE";
        private const string MarketSummariesUpdate = "uS";
        private const string MarketSummariesLiteUpdate = "uL";
        private const string BalanceUpdate = "uB";
        private const string OrderUpdate = "uO";
        #endregion

        #region ctor
        /// <summary>
        /// Creates a new socket client using the default options
        /// </summary>
        public BittrexSocketClient(): this(DefaultOptions)
        {
        }

        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClient(BittrexSocketClientOptions options): base(options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            SocketFactory = new ConnectionFactory();

            SocketCombineTarget = 10;
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
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        public CallResult<IEnumerable<BittrexStreamMarketSummary>> QuerySummaryStates() => QuerySummaryStatesAsync().Result;

        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        public async Task<CallResult<IEnumerable<BittrexStreamMarketSummary>>> QuerySummaryStatesAsync()
        {
            var result = await Query<BittrexStreamMarketSummariesQuery>(new ConnectionRequest(QuerySummaryStateRequest), false).ConfigureAwait(false);
            return new CallResult<IEnumerable<BittrexStreamMarketSummary>>(result.Data?.Deltas, result.Error);
        }

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        public CallResult<BittrexStreamQueryExchangeState> QueryExchangeState(string symbol) => QueryExchangeStateAsync(symbol).Result;

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        public async Task<CallResult<BittrexStreamQueryExchangeState>> QueryExchangeStateAsync(string symbol)
        {
            symbol.ValidateBittrexSymbol();

            return await Query<BittrexStreamQueryExchangeState>(new ConnectionRequest(QueryExchangeStateRequest, symbol), false).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="symbol">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToExchangeStateUpdates(string symbol, Action<BittrexStreamUpdateExchangeState> onUpdate) => SubscribeToExchangeStateUpdatesAsync(symbol, onUpdate).Result;

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="symbol">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToExchangeStateUpdatesAsync(string symbol, Action<BittrexStreamUpdateExchangeState> onUpdate)
        {
            symbol.ValidateBittrexSymbol();

            return await Subscribe<JToken>(new ConnectionRequest(ExchangeDeltaSub, symbol), null, false, data => DecodeSignalRData(data, onUpdate)).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToMarketSummariesUpdate(Action<IEnumerable<BittrexStreamMarketSummary>> onUpdate) => SubscribeToMarketSummariesUpdateAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesUpdateAsync(Action<IEnumerable<BittrexStreamMarketSummary>> onUpdate)
        {
            var inner = new Action<BittrexStreamMarketSummaryUpdate>(data => onUpdate(data.Deltas));
            return await Subscribe<JToken>(new ConnectionRequest(SummaryDeltaSub), null, false, data => DecodeSignalRData(data, inner)).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToMarketSummariesLiteUpdate(Action<IEnumerable<BittrexStreamMarketSummaryLite>> onUpdate) => SubscribeToMarketSummariesLiteUpdateAsync(onUpdate).Result;

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesLiteUpdateAsync(Action<IEnumerable<BittrexStreamMarketSummaryLite>> onUpdate)
        {
            var inner = new Action<BittrexStreamMarketSummariesLite>(data => onUpdate(data.Deltas));
            return await Subscribe<JToken>(new ConnectionRequest(SummaryLiteDeltaSub), null, false, data => DecodeSignalRData(data, inner)).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onBalanceUpdate">The update event handler for balance updates</param>
        /// <param name="onOrderUpdate">The update event handler for order updates</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToAccountUpdates(Action<BittrexStreamBalanceData> onBalanceUpdate, Action<BittrexStreamOrderData> onOrderUpdate) => SubscribeToAccountUpdatesAsync(onBalanceUpdate, onOrderUpdate).Result;

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onBalanceUpdate">The update event handler for balance updates</param>
        /// <param name="onOrderUpdate">The update event handler for order updates</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToAccountUpdatesAsync(Action<BittrexStreamBalanceData> onBalanceUpdate, Action<BittrexStreamOrderData> onOrderUpdate)
        {
            var handler = new Action<JToken>(token =>
            {
                if(token["d"] != null)
                {
                    var desResult = Deserialize<BittrexStreamBalanceData>(token);
                    if (!desResult.Success)
                    {
                        log.Write(LogVerbosity.Warning, "Failed to deserialize balance update: " + desResult.Error);
                        return;
                    }

                    onBalanceUpdate(desResult.Data);
                }
                else
                {
                    var desResult = Deserialize<BittrexStreamOrderData>(token);
                    if (!desResult.Success)
                    {
                        log.Write(LogVerbosity.Warning, "Failed to deserialize order update: " + desResult.Error);
                        return;
                    }

                    onOrderUpdate(desResult.Data);
                }
            });

            return await Subscribe<JToken>(null, "AccountUpdates", true, data => DecodeSignalRData(data, handler)).ConfigureAwait(false);
        }
        #endregion
        #region private
        /// <inheritdoc />
        protected override SocketConnection GetWebsocket(string address, bool authenticated)
        {

            // Override because signalr puts `/signalr/` add the end of the url
            var socketResult = sockets.Where(s => s.Value.Socket.Url == address + "/signalr/" && (s.Value.Authenticated == authenticated || !authenticated) && s.Value.Connected).OrderBy(s => s.Value.HandlerCount).FirstOrDefault();
            var result = socketResult.Equals(default(KeyValuePair<int, SocketConnection>)) ? null : socketResult.Value;
            if (result != null)
            {
                if (result.HandlerCount < SocketCombineTarget || (sockets.Count >= MaxSocketConnections && sockets.All(s => s.Value.HandlerCount >= SocketCombineTarget)))
                {
                    // Use existing socket if it has less than target connections OR it has the least connections and we can't make new
                    return result;
                }
            }

            // Create new socket
            var socket = CreateSocket(address);
            var socketWrapper = new SocketConnection(this, socket);
            foreach (var kvp in genericHandlers)
                socketWrapper.AddHandler(SocketSubscription.CreateForIdentifier(kvp.Key, false, kvp.Value));
            return socketWrapper;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> SubscribeAndWait(SocketConnection socket, object request, SocketSubscription subscription)
        {
            var btRequest = (ConnectionRequest) request;
            if (btRequest.RequestName != null)
            {
                var subResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<bool>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
                if (!subResult.Success || !subResult.Data)
                {
                    _ = socket.Close(subscription);
                    return new CallResult<bool>(false, subResult.Error ?? new ServerError("Subscribe returned false"));
                }
            }

            subscription.Confirmed = true;
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<T>> QueryAndWait<T>(SocketConnection socket, object request)
        {
            var btRequest = (ConnectionRequest) request;
            var queryResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<string>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                return new CallResult<T>(default, queryResult.Error);
            }

            var decResult = DecodeData(queryResult.Data);
            if (decResult == null)
            {
                return new CallResult<T>(default, new DeserializeError("Failed to decode data"));
            }

            var desResult = Deserialize<T>(decResult);
            if (!desResult.Success)
            {
                return new CallResult<T>(default, desResult.Error);
            }

            return new CallResult<T>(desResult.Data, null);
        }

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string) message["M"];
            var data = DecodeData((string) message["A"][0]);
            if (data == null)
                return false;

            var btRequest = (ConnectionRequest) request;
            if (btRequest.RequestName == ExchangeDeltaSub && method == ExchangeStateUpdate)
            {
                var token = JToken.Parse(data);
                if ((string)token["M"] == btRequest.Parameters[0].ToString())
                    return true;
            }

            if (btRequest.RequestName == SummaryDeltaSub && method == MarketSummariesUpdate)
                return true;

            if (btRequest.RequestName == SummaryLiteDeltaSub && method == MarketSummariesLiteUpdate)
                return true;

            return false;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string)message["M"];

            if (identifier == "AccountUpdates" && (method == BalanceUpdate || method == OrderUpdate))
                return true;
            return false;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            log.Write(LogVerbosity.Debug, "Starting authentication");
            var socket = (ISignalRSocket)s.Socket;
            var result = await socket.InvokeProxy<string>("GetAuthContext", authProvider.Credentials.Key!.GetString()).ConfigureAwait(false);
            if (!result.Success)
            {
                log.Write(LogVerbosity.Error, "Api key is probably invalid");
                return new CallResult<bool>(false, result.Error);
            }

            log.Write(LogVerbosity.Debug, "Auth context retrieved");
            var signed = authProvider.Sign(result.Data);
            var authResult = await socket.InvokeProxy<bool>("Authenticate", authProvider.Credentials.Key!.GetString(), signed).ConfigureAwait(false);
            if (!authResult.Success || !authResult.Data)
            {
                log.Write(LogVerbosity.Error, "Authentication failed, api secret is probably invalid");
                return new CallResult<bool>(false, authResult.Error ?? new ServerError("Api secret is probably invalid"));
            }

            log.Write(LogVerbosity.Info, "Authentication successful");
            return new CallResult<bool>(true, null);
        }

        /// <summary>
        /// Unsubscribe from a stream
        /// * NOT SUPPORTED BY BITTREX' CURRENT SOCKET IMPLEMENTATION *
        /// </summary>
        /// <param name="subscription">The subscription to unsubscribe</param>
        /// <returns></returns>
        public override Task Unsubscribe(UpdateSubscription subscription)
        {
            throw new NotImplementedException("Bittrex sockets do not offer unsubscription functionality");
        }

        /// <inheritdoc />
        protected override Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override IWebsocket CreateSocket(string address)
        {
            var socket = (ISignalRSocket)base.CreateSocket(address);
            socket.SetHub(HubName);
            return socket;
        }

        private void DecodeSignalRData<T>(JToken data, Action<T> handler)
        {
            var actualData = (string)data["A"][0];
            var result = DecodeData(actualData);
            if (result == null)
                return;

            log.Write(LogVerbosity.Debug, "Socket received data: " + result);

            var decodeResult = Deserialize<T>(result);
            if (!decodeResult.Success)
                log.Write(LogVerbosity.Debug, "Failed to decode data: " + decodeResult.Error);

            handler(decodeResult.Data);
        }

        private string? DecodeData(string rawData)
        {
            try
            {
                var gzipData = Convert.FromBase64String(rawData);
                using var decompressedStream = new MemoryStream();
                using var compressedStream = new MemoryStream(gzipData);
                using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
                deflateStream.CopyTo(decompressedStream);
                decompressedStream.Position = 0;

                using var streamReader = new StreamReader(decompressedStream);
                var data = streamReader.ReadToEnd();
                if (data == "null")
                    return null;

                return data;
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Info, "Exception in decode data: " + e.Message);
                return null;
            }
        }
        #endregion
        #endregion
    }
}
