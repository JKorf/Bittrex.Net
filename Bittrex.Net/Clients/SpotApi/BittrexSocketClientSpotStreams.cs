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

namespace Bittrex.Net.Clients.SpotApi
{
    /// <summary>
    /// Client for the Bittrex V3 websocket API
    /// </summary>
    public class BittrexSocketClientSpotStreams : SocketApiClient, IBittrexSocketClientSpotStreams
    {
        #region fields
        private const string HubName = "c3";

        private readonly BittrexSocketClient _baseClient;
        #endregion

        #region ctor
        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClientSpotStreams(BittrexSocketClient baseClient, BittrexSocketClientOptions options) :
            base(options, options.SpotStreamOptions)
        {
            _baseClient = baseClient;
        }
        #endregion

        public override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BittrexAuthenticationProvider(credentials);


        #region methods
        #region public
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "heartbeat", false, onHeartbeat, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, symbols.Select(s => $"candle_{s}_{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}").ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "market_summaries", false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default)
            => SubscribeToSymbolSummaryUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, symbols.Select(s => "market_summary_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, depth, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 1, 25, 500);
            return await _baseClient.SubscribeInternalAsync(this, symbols.Select(s => $"orderbook_{s}_{depth}").ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "tickers", false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default) => SubscribeToTickerUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, symbols.Select(s => "ticker_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { symbol }, onUpdate, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, symbols.Select(s => "trade_" + s).ToArray(), false, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "order", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "balance", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<BittrexExecutionUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "execution", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "deposit", true, onUpdate, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToConditionalOrderUpdatesAsync(Action<DataEvent<BittrexConditionalOrderUpdate>> onUpdate, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, "conditional_order", true, onUpdate, ct).ConfigureAwait(false);
        }

        #endregion
        #endregion
    }
}
