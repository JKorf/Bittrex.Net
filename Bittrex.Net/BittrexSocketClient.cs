using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Bittrex.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net.Authentication;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex V3 websocket API
    /// </summary>
    public class BittrexSocketClient: SocketClient, IBittrexSocketClient
    {
        #region fields
        private static BittrexSocketClientOptions defaultOptions = new BittrexSocketClientOptions();
        private static BittrexSocketClientOptions DefaultOptions => defaultOptions.Copy<BittrexSocketClientOptions>();

        private const string HubName = "c3";
        
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
        public BittrexSocketClient(BittrexSocketClientOptions options): base("Bittrex", options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            SocketFactory = new ConnectionFactory(options.Proxy);

            SocketCombineTarget = 10;

            AddGenericHandler("Reauthenticate", async (messageEvent) => await AuthenticateSocketAsync(messageEvent.Connection).ConfigureAwait(false));
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new BittrexAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }

        /// <summary>
        /// Set the default options for new clients
        /// </summary>
        /// <param name="options">Options to use for new clients</param>
        public static void SetDefaultOptions(BittrexSocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Subscribe to heartbeat updates
        /// </summary>
        /// <param name="onHeartbeat">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat)
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
            KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate)
            => SubscribeToKlineUpdatesAsync(new[] {symbol}, interval, onUpdate);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate)
        {
            return await Subscribe(symbols.Select(s => $"candle_{s}_{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}").ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to all symbol summary updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate)
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
            Action<DataEvent<BittrexSymbolSummary>> onUpdate)
            => SubscribeToSymbolSummaryUpdatesAsync(new[] { symbol }, onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate)
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
            Action<DataEvent<BittrexOrderBookUpdate>> onUpdate)
            => SubscribeToOrderBookUpdatesAsync(new[] {symbol}, depth, onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate)
        {
            depth.ValidateIntValues(nameof(depth), 1, 25, 500);
            return await Subscribe(symbols.Select(s => $"orderbook_{s}_{depth}").ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to all symbols ticker updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate)
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
            Action<DataEvent<BittrexTick>> onUpdate) => SubscribeToSymbolTickerUpdatesAsync(new[] {symbol}, onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate)
        {
            return await Subscribe(symbols.Select(s => "ticker_" + s).ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTradesUpdate>> onUpdate)
            => SubscribeToTradeUpdatesAsync(new[] {symbol}, onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate)
        {
            return await Subscribe(symbols.Select(s => "trade_" + s).ToArray(), false, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate)
        {
            return await Subscribe("order", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to balance updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate)
        {
            return await Subscribe("balance", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to execution updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BittrexExecutionUpdate>> onUpdate)
        {
            return await Subscribe("execution", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to deposit updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate)
        {
            return await Subscribe("deposit", true, onUpdate).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to conditional order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToConditionalOrderUpdatesAsync(Action<DataEvent<BittrexConditionalOrderUpdate>> onUpdate)
        {
            return await Subscribe("conditional_order", true, onUpdate).ConfigureAwait(false);
        }

        #endregion
        #region private

        private Task<CallResult<UpdateSubscription>> Subscribe<T>(string channel, bool authenticated,
            Action<DataEvent<T>> handler)
            => Subscribe(new[] {channel}, authenticated, handler);

        private async Task<CallResult<UpdateSubscription>> Subscribe<T>(string[] channels, bool authenticated, Action<DataEvent<T>> handler)
        {
            return await base.SubscribeAsync<JToken>(new ConnectionRequest("subscribe", channels), null, authenticated, data =>
            {
                if ((string) data.Data["M"] == "heartbeat")
                {
                    handler(data.As((T) Convert.ChangeType(DateTime.UtcNow, typeof(T))));
                    return;
                }

                if (!data.Data["A"].Any())
                    return;
                DecodeSignalRData(data, handler);
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override SocketConnection GetSocketConnection(string address, bool authenticated)
        {
            // Override because signalr puts `/signalr/` add the end of the url
            var socketResult = sockets.Where(s => s.Value.Socket.Url == address + "/signalr/" && (s.Value.Authenticated == authenticated || !authenticated) && s.Value.Connected).OrderBy(s => s.Value.SubscriptionCount).FirstOrDefault();
            var result = socketResult.Equals(default(KeyValuePair<int, SocketConnection>)) ? null : socketResult.Value;
            if (result != null)
            {
                if (result.SubscriptionCount < SocketCombineTarget || (sockets.Count >= MaxSocketConnections && sockets.All(s => s.Value.SubscriptionCount >= SocketCombineTarget)))
                {
                    // Use existing socket if it has less than target connections OR it has the least connections and we can't make new
                    return result;
                }
            }

            // Create new socket
            var socket = CreateSocket(address);
            var socketWrapper = new SocketConnection(this, socket);
            foreach (var kvp in genericHandlers)
                socketWrapper.AddSubscription(SocketSubscription.CreateForIdentifier(NextId(), kvp.Key, false, kvp.Value));
            return socketWrapper;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> SubscribeAndWaitAsync(SocketConnection socket, object request, SocketSubscription subscription)
        {
            var btRequest = (ConnectionRequest) request;
            if (btRequest.RequestName != null)
            {
                var subResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<ConnectionResponse[]>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
                var data = subResult.Data?.First();
                if (!subResult.Success || data?.Success == false)
                {
                    _ = socket.CloseAsync(subscription);
                    return new CallResult<bool>(false, subResult.Error ?? new ServerError(data?.ErrorCode!));
                }
            }

            subscription.Confirmed = true;
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<T>> QueryAndWaitAsync<T>(SocketConnection socket, object request)
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

            var bRequest = (ConnectionRequest) request;

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

        private static bool Check(string channel, string method, string data)
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
        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            if (authProvider == null || authProvider.Credentials?.Key == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var randomContent = $"{ Guid.NewGuid() }";
            var content = string.Join("", timestamp, randomContent);
            var signedContent = authProvider.Sign(content);
            var socket = (ISignalRSocket)s.Socket;

            var result = await socket.InvokeProxy<ConnectionResponse>("Authenticate", authProvider.Credentials.Key.GetString(), timestamp, randomContent, signedContent).ConfigureAwait(false);
            if (!result.Success || !result.Data.Success)
            {
                log.Write(LogLevel.Error, "Authentication failed, api key/secret is probably invalid");
                return new CallResult<bool>(false, result.Error ?? new ServerError("Authentication failed. Api key/secret is probably invalid"));
            }

            log.Write(LogLevel.Information, "Authentication successful");
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription s)
        {
            var bRequest = (ConnectionRequest)s.Request!;
            var unsub = new ConnectionRequest("unsubscribe", ((string[])bRequest!.Parameters[0])[0]);
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

        private void DecodeSignalRData<T>(DataEvent<JToken> data, Action<DataEvent<T>> handler)
        {
            var actualData = (string)data.Data["A"][0];
            var result = DecodeData(actualData);
            if (result == null)
                return;

            log.Write(LogLevel.Debug, "Socket received data: " + result);

            var token = result.ToJToken(log);
            if (token == null)
                return;

            string? symbol = null;
            if (token["marketSymbol"] != null)
                symbol = (string)token["marketSymbol"];
            else if (token["symbol"] != null)
                symbol = (string)token["symbol"];
            else if (token["deltas"] != null && token["deltas"][0]["marketSymbol"] != null)
                symbol = (string)token["deltas"][0]["marketSymbol"];
            else if (token["deltas"] != null && token["deltas"][0]["symbol"] != null)
                symbol = (string)token["deltas"][0]["symbol"];


            var decodeResult = Deserialize<T>(token);
            if (!decodeResult.Success)
                log.Write(LogLevel.Debug, "Failed to decode data: " + decodeResult.Error);

            handler(data.As(decodeResult.Data, symbol));
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
                log.Write(LogLevel.Warning, "Exception in decode data: " + e.ToLogString());
                return null;
            }
        }
        #endregion
        #endregion
    }
}
