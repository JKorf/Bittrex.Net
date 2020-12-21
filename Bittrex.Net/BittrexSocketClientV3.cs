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
using System.Text.RegularExpressions;
using Bittrex.Net.Converters.V3;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex V3 websocket API
    /// </summary>
    public class BittrexSocketClientV3: SocketClient, IBittrexSocketClientV3
    {
        #region fields
        private static BittrexSocketClientV3Options defaultOptions = new BittrexSocketClientV3Options();
        private static BittrexSocketClientV3Options DefaultOptions => defaultOptions.Copy<BittrexSocketClientV3Options>();

        private const string HubName = "c3";
        
        #endregion

        #region ctor
        /// <summary>
        /// Creates a new socket client using the default options
        /// </summary>
        public BittrexSocketClientV3(): this(DefaultOptions)
        {
        }

        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClientV3(BittrexSocketClientV3Options options): base("Bittrex", options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            SocketFactory = new ConnectionFactory();

            SocketCombineTarget = 10;

            AddGenericHandler("Reauthenticate", async (connection, data) => await AuthenticateSocket(connection).ConfigureAwait(false));
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Set the default options for new clients
        /// </summary>
        /// <param name="options">Options to use for new clients</param>
        public static void SetDefaultOptions(BittrexSocketClientV3Options options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Subscribe to heartbeat updates
        /// </summary>
        /// <param name="onHeartbeat">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DateTime> onHeartbeat)
        {
            return await Subscribe("heartbeat", false, onHeartbeat).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<BittrexKlineUpdate> onUpdate)
            => SubscribeToKlineUpdatesAsync(new[] {symbol}, interval, onUpdate);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<BittrexKlineUpdate> onUpdate)
        {
            return await Subscribe(symbols.Select(s => $"candle_{s}_{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}").ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to all symbol summary updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<BittrexSummariesUpdate> onUpdate)
        {
            return await Subscribe("market_summaries", false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<BittrexSymbolSummaryV3> onUpdate)
            => SubscribeToSymbolSummaryUpdatesAsync(new[] { symbol }, onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<BittrexSymbolSummaryV3> onUpdate)
        {
            return await Subscribe(symbols.Select(s => "market_summary_" + s).ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public  Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<BittrexOrderBookUpdate> onUpdate)
            => SubscribeToOrderBookUpdatesAsync(new[] {symbol}, depth, onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<BittrexOrderBookUpdate> onUpdate)
        {
            depth.ValidateIntValues(nameof(depth), 1, 25, 500);
            return await Subscribe(symbols.Select(s => $"orderbook_{s}_{depth}").ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to all symbols ticker updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(Action<BittrexTickersUpdate> onUpdate)
        {
            return await Subscribe("tickers", false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(string symbol,
            Action<BittrexTickV3> onUpdate) => SubscribeToSymbolTickerUpdatesAsync(new[] {symbol}, onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(IEnumerable<string> symbols, Action<BittrexTickV3> onUpdate)
        {
            return await Subscribe(symbols.Select(s => "ticker_" + s).ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(string symbol,
            Action<BittrexTradesUpdate> onUpdate)
            => SubscribeToSymbolTradeUpdatesAsync(new[] {symbol}, onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(IEnumerable<string> symbols, Action<BittrexTradesUpdate> onUpdate)
        {
            return await Subscribe(symbols.Select(s => "trade_" + s).ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<BittrexOrderUpdate> onUpdate)
        {
            return await Subscribe("order", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to balance updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<BittrexBalanceUpdate> onUpdate)
        {
            return await Subscribe("balance", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to deposit updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<BittrexDepositUpdate> onUpdate)
        {
            return await Subscribe("deposit", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to conditional order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToConditionalOrderUpdatesAsync(Action<BittrexConditionalOrderUpdate> onUpdate)
        {
            return await Subscribe("conditional_order", true, onUpdate).ConfigureAwait(false);
        }

        #endregion
        #region private

        private Task<CallResult<UpdateSubscription>> Subscribe<T>(string channel, bool authenticated,
            Action<T> handler)
            => Subscribe(new[] {channel}, authenticated, handler);

        private async Task<CallResult<UpdateSubscription>> Subscribe<T>(string[] channels, bool authenticated, Action<T> handler)
        {
            return await Subscribe<JToken>(new ConnectionRequestV3("subscribe", channels), null, authenticated, data =>
            {
                if ((string) data["M"] == "heartbeat")
                {
                    handler((T) Convert.ChangeType(DateTime.UtcNow, typeof(T)));
                    return;
                }

                if (!data["A"].Any())
                    return;
                DecodeSignalRData(data, handler);
            }).ConfigureAwait(false);
        }

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
            var btRequest = (ConnectionRequestV3) request;
            if (btRequest.RequestName != null)
            {
                var subResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<ConnectionResponse[]>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
                var data = subResult.Data?.First();
                if (!subResult.Success || data?.Success == false)
                {
                    _ = socket.Close(subscription);
                    return new CallResult<bool>(false, subResult.Error ?? new ServerError(data?.ErrorCode!));
                }
            }

            subscription.Confirmed = true;
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<T>> QueryAndWait<T>(SocketConnection socket, object request)
        {
            var btRequest = (ConnectionRequestV3) request;
            var queryResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<string>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                return new CallResult<T>(default, queryResult.Error);
            }

            var decResult = DecodeData(queryResult.Data);
            if (decResult == null)
            {
                return new CallResult<T>(default, new DeserializeError("Failed to decode data", queryResult.Data));
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

            var method = (string?) message["M"];
            method = string.Join("_", Regex.Split(method, @"(?<!^)(?=[A-Z])").Select(s => s.ToLower()));
            if (method == "heartbeat")
                return true;

            var arguments = (string?) message["A"].FirstOrDefault();
            if (arguments == null)
                return false;

            var data = DecodeData(arguments);
            if (data == null)
                return method == "heartbeat";

            var bRequest = (ConnectionRequestV3) request;

            var m = method.Replace("order_book", "orderbook");

            foreach (var parameter in bRequest.Parameters)
            {
                foreach (var channel in (string[]) parameter)
                {

                    if (Check(channel, m, data))
                        return true;
                }
            }

            return false;
        }

        private bool Check(string channel, string method, string data)
        {
            if (channel == method)
                return true;
            
            if (channel.StartsWith(method))
            {
                var tokenData = JToken.Parse(data);
                var symbol = (string)(tokenData["symbol"] ?? tokenData["marketSymbol"]);
                if (channel.Length < method.Length + symbol.Length + 1)
                    return false;

                if (channel.StartsWith("candle") && method == "candle")
                {
                    var interval = (string)tokenData["interval"];
                    return channel.Substring(method.Length + 1, symbol.Length) == symbol && channel.EndsWith(interval);
                }

                if (channel.Substring(method.Length + 1, symbol.Length) == symbol)
                    return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string)message["M"];
            if (method == "authenticationExpiring" && identifier == "Reauthenticate")
                return true;
            return false;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            if (authProvider == null || authProvider.Credentials?.Key == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var randomContent = $"{ Guid.NewGuid() }";
            var content = string.Join("", timestamp, randomContent);
            var signedContent = authProvider.Sign(content);
            var socket = (ISignalRSocket)s.Socket;

            var result = await socket.InvokeProxy<ConnectionResponse>("Authenticate", authProvider.Credentials.Key.GetString(), timestamp, randomContent, signedContent).ConfigureAwait(false);
            if (!result.Success)
            {
                log.Write(LogVerbosity.Error, "Authentication failed, api key/secret is probably invalid");
                return new CallResult<bool>(false, result.Error ?? new ServerError("Authentication failed. Api key/secret is probably invalid"));
            }

            log.Write(LogVerbosity.Info, "Authentication successful");
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            var bRequest = (ConnectionRequestV3)s.Request!;
            var unsub = new ConnectionRequestV3("unsubscribe", ((string[])bRequest!.Parameters[0])[0]);
            var queryResult = await ((ISignalRSocket)connection.Socket).InvokeProxy<ConnectionResponse[]>(unsub.RequestName, unsub.Parameters).ConfigureAwait(false);
            
            return queryResult.Success;
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
