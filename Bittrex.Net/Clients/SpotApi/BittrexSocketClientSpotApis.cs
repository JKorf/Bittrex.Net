using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using System.Linq;
using Bittrex.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using CryptoExchange.Net.Authentication;
using Bittrex.Net.Enums;
using System.Threading;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Objects.Models.Socket;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Newtonsoft.Json.Linq;
using Bittrex.Net.Objects.Internal;
using Bittrex.Net.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net.Interfaces;
using System.IO;
using System.IO.Compression;
using Bittrex.Net.Objects.Options;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBittrexSocketClientSpotApi" />
    public class BittrexSocketClientSpotApi : SocketApiClient, IBittrexSocketClientSpotApi
    {
        #region fields
        private const string _hubName = "c3";
        #endregion

        #region ctor
        internal BittrexSocketClientSpotApi(ILogger log, BittrexSocketOptions options) :
            base(log, options.Environment.SocketAddress, options, options.SpotOptions)
        {
            SocketFactory = new BittrexWebsocketFactory();

            AddGenericHandler("Reauthenticate", async (messageEvent) => await AuthenticateSocketAsync(messageEvent.Connection).ConfigureAwait(false));
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BittrexAuthenticationProvider(credentials);


        #region methods
        #region public
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat, CancellationToken ct = default)
        {
            return await SubscribeAsync("heartbeat", false, onHeartbeat, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync(symbols.Select(s => $"candle_{s}_{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}").ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("market_summaries", false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default)
            => SubscribeToSymbolSummaryUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync(symbols.Select(s => "market_summary_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, depth, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 1, 25, 500);
            return await SubscribeAsync(symbols.Select(s => $"orderbook_{s}_{depth}").ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("tickers", false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default) => SubscribeToTickerUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync(symbols.Select(s => "ticker_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync(symbols.Select(s => "trade_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("order", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("balance", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BittrexExecutionUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("execution", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("deposit", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToConditionalOrderUpdatesAsync(Action<DataEvent<BittrexConditionalOrderUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await SubscribeAsync("conditional_order", true, onUpdate, ct).ConfigureAwait(false);
        }

        #endregion

        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string channel, bool authenticated,
            Action<DataEvent<T>> handler, CancellationToken ct)
            => SubscribeAsync(new[] { channel }, authenticated, handler, ct);

        internal async Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string[] channels, bool authenticated, Action<DataEvent<T>> handler, CancellationToken ct)
        {
            return await base.SubscribeAsync<JToken>(new ConnectionRequest("subscribe", channels), null, authenticated, data =>
            {
                if (data.Data["M"]?.ToString() == "heartbeat" && channels[0] == "heartbeat")
                {
                    handler(data.As((T)Convert.ChangeType(DateTime.UtcNow, typeof(T))));
                    return;
                }

                if (data.Data["A"]?.Any() != true)
                    return;
                DecodeSignalRData(data, handler);
            }, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> SubscribeAndWaitAsync(SocketConnection socket, object request, SocketSubscription subscription)
        {
            var btRequest = (ConnectionRequest)request;
            if (btRequest.RequestName != null)
            {
                var subResult = await ((ISignalRSocket)socket.GetSocket()).InvokeProxy<ConnectionResponse[]>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
                var data = subResult.Data?.First();
                if (!subResult.Success || data?.Success == false)
                {
                    _ = socket.CloseAsync(subscription);
                    return new CallResult<bool>(subResult.Error ?? new ServerError(data?.ErrorCode!));
                }
            }

            subscription.Confirmed = true;
            return new CallResult<bool>(true);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<T>> QueryAndWaitAsync<T>(SocketConnection socket, object request, int weight)
        {
            var btRequest = (ConnectionRequest)request;
            var queryResult = await ((ISignalRSocket)socket.GetSocket()).InvokeProxy<string>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                return new CallResult<T>(queryResult.Error!);
            }

            var decResult = DecodeData(queryResult.Data);
            if (decResult == null)
            {
                return new CallResult<T>(new DeserializeError("Failed to decode data", queryResult.Data));
            }

            var desResult = Deserialize<T>(decResult);
            if (!desResult.Success)
            {
                return new CallResult<T>(desResult.Error!);
            }

            return new CallResult<T>(desResult.Data);
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
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string?)message["M"];
            if (method == null)
                return false;

            method = string.Join("_", Regex.Split(method, @"(?<!^)(?=[A-Z])").Select(s => s.ToLower()));
            if (method == "heartbeat")
                return true;

            var arguments = (string?)msg.FirstOrDefault();
            if (arguments == null)
                return false;

            var data = DecodeData(arguments);
            if (data == null)
                return method == "heartbeat";

            var bRequest = (ConnectionRequest)request;

            var m = method.Replace("order_book", "orderbook");

            foreach (var parameter in bRequest.Parameters)
            {
                foreach (var channel in (string[])parameter)
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
                var symbol = (tokenData["symbol"] ?? tokenData["marketSymbol"])?.ToString();
                if (symbol == null)
                    return false;

                if (channel.Length < method.Length + symbol.Length + 1)
                    return false;

                if (channel.StartsWith("candle") && method == "candle")
                {
                    var interval = tokenData["interval"]?.ToString();
                    if (interval == null)
                        return false;

                    return channel.Substring(method.Length + 1, symbol.Length) == symbol && channel.EndsWith(interval);
                }

                if (channel.StartsWith("orderbook") && method == "orderbook")
                {
                    var depth = tokenData["depth"]?.ToString();
                    if (depth == null)
                        return false;

                    var segments = channel.Split('_');
                    return segments.ElementAtOrDefault(1) == symbol && segments.ElementAtOrDefault(2) == depth;
                }

                if (channel.Substring(method.Length + 1, channel.Length - (method.Length + 1)) == symbol)
                    return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = message["M"]?.ToString();
            if (method == "authenticationExpiring" && identifier == "Reauthenticate")
                return true;
            return false;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            if (s.ApiClient.AuthenticationProvider == null)
                return new CallResult<bool>(new NoApiCredentialsError());

            var bittrexAuthProvider = (BittrexAuthenticationProvider)s.ApiClient.AuthenticationProvider;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var randomContent = $"{Guid.NewGuid()}";
            var content = string.Join("", timestamp, randomContent);
            var signedContent = bittrexAuthProvider.Sign(content);
            var socket = (ISignalRSocket)s.GetSocket();

            var result = await socket.InvokeProxy<ConnectionResponse>("Authenticate", bittrexAuthProvider.GetApiKey(), timestamp, randomContent, signedContent).ConfigureAwait(false);
            if (!result.Success || !result.Data.Success)
            {
                _logger.Log(LogLevel.Error, $"Socket {s.SocketId} Authentication failed, api key/secret is probably invalid");
                return new CallResult<bool>(result.Error ?? new ServerError("Authentication failed. Api key/secret is probably invalid"));
            }

            _logger.Log(LogLevel.Debug, $"Socket {s.SocketId} Authentication successful");
            return new CallResult<bool>(true);
        }

        /// <inheritdoc />
        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription s)
        {
            var bRequest = (ConnectionRequest)s.Request!;
            var unsub = new ConnectionRequest("unsubscribe", ((string[])bRequest!.Parameters[0])[0]);
            var queryResult = await ((ISignalRSocket)connection.GetSocket()).InvokeProxy<ConnectionResponse[]>(unsub.RequestName, unsub.Parameters).ConfigureAwait(false);

            return queryResult.Success;
        }

        /// <inheritdoc />
        protected override IWebsocket CreateSocket(string address)
        {
            var socket = (ISignalRSocket)base.CreateSocket(address);
            socket.SetHub(_hubName);
            return socket;
        }

        private void DecodeSignalRData<T>(DataEvent<JToken> data, Action<DataEvent<T>> handler)
        {
            var internalData = data.Data["A"];
            if (internalData == null || !internalData.Any())
            {
                _logger.Log(LogLevel.Warning, "Received update without data? " + data.Data);
                return;
            }

            var actualData = internalData[0]?.ToString();
            if (actualData == null)
            {
                _logger.Log(LogLevel.Warning, "Received update without actual data? " + data.Data);
                return;
            }

            var result = DecodeData(actualData);
            if (result == null)
                return;

            _logger.Log(LogLevel.Trace, "Socket received data: " + result);

            var tokenResult = ValidateJson(result);
            if (!tokenResult)
                return;

            var token = tokenResult.Data;
            string? symbol = null;
            if (token["marketSymbol"] != null)
                symbol = token["marketSymbol"]?.ToString();
            else if (token["symbol"] != null)
                symbol = token["symbol"]?.ToString();
            else if (token["deltas"]?.Count() > 0 && token["deltas"]![0]!["marketSymbol"] != null)
                symbol = token["deltas"]![0]!["marketSymbol"]?.ToString();
            else if (token["deltas"]?.Count() > 0 && token["deltas"]![0]!["symbol"] != null)
                symbol = token["deltas"]![0]!["symbol"]?.ToString();


            var decodeResult = Deserialize<T>(token);
            if (!decodeResult.Success)
                _logger.Log(LogLevel.Debug, "Failed to decode data: " + decodeResult.Error);

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
                _logger.Log(LogLevel.Warning, "Exception in decode data: " + e.ToLogString());
                return null;
            }
        }
        #endregion
    }
}
