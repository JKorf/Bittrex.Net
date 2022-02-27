using Bittrex.Net.Enums;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBittrexClientSpotApi" />
    public class BittrexClientSpotApi : RestApiClient, IBittrexClientSpotApi, ISpotClient
    {
        private readonly BittrexClient _baseClient;
        private readonly BittrexClientOptions _options;
        private readonly Log _log;

        internal static TimeSyncState TimeSyncState = new TimeSyncState();

        internal static TimeSpan TimeOffset;
        internal static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);
        internal static DateTime LastTimeSync;

        /// <inheritdoc />
        public string ExchangeName => "Bittrex";

        #region Api clients

        /// <inheritdoc />
        public IBittrexClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IBittrexClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBittrexClientSpotApiTrading Trading { get; }

        #endregion

        internal BittrexClientSpotApi(Log log, BittrexClient baseClient, BittrexClientOptions options) :
            base(options, options.SpotApiOptions)
        {
            _options = options;
            _log = log;
            _baseClient = baseClient;

            Account = new BittrexClientSpotApiAccount(this);
            ExchangeData = new BittrexClientSpotApiExchangeData(this);
            Trading = new BittrexClientSpotApiTrading(this);
        }

        /// <summary>
        /// Event triggered when an order is placed via this client
        /// </summary>
        public event Action<OrderId>? OnOrderPlaced;
        /// <summary>
        /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOpenOrdersAsync
        /// </summary>
        public event Action<OrderId>? OnOrderCanceled;

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BittrexAuthenticationProvider(credentials);

        #region common interface
        async Task<WebCallResult<IEnumerable<Symbol>>> IBaseRestClient.GetSymbolsAsync(CancellationToken ct)
        {
            var symbols = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!symbols)
                return symbols.As<IEnumerable<Symbol>>(null);

            return symbols.As(symbols.Data.Select(s => new Symbol
            {
                SourceObject = s,
                Name = s.Name,
                MinTradeQuantity = s.MinTradeQuantity,
                PriceDecimals = s.Precision
            }));
        }

        async Task<WebCallResult<OrderBook>> IBaseRestClient.GetOrderBookAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetOrderBookAsync), nameof(symbol));

            var orderBookResult = await ExchangeData.GetOrderBookAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orderBookResult)
                return orderBookResult.As<OrderBook>(null);

            return orderBookResult.As(new OrderBook
            {
                SourceObject = orderBookResult.Data,
                Asks = orderBookResult.Data.Asks.Select(a => new OrderBookEntry { Price = a.Price, Quantity = a.Quantity }),
                Bids = orderBookResult.Data.Bids.Select(b => new OrderBookEntry { Price = b.Price, Quantity = b.Quantity })
            });
        }

        async Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetTickerAsync), nameof(symbol));

            var ticker = await ExchangeData.GetSymbolSummaryAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!ticker)
                return ticker.As<Ticker>(null);

            return ticker.As(new Ticker
            {
                SourceObject = ticker.Data,
                Symbol = ticker.Data.Symbol,
                HighPrice = ticker.Data.HighPrice,
                LowPrice = ticker.Data.LowPrice,
                Volume = ticker.Data.Volume
            });
        }

        async Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            var tickers = await ExchangeData.GetSymbolSummariesAsync(ct: ct).ConfigureAwait(false);
            if (!tickers)
                return tickers.As<IEnumerable<Ticker>>(null);

            return tickers.As(tickers.Data.Select(t => new Ticker
            {
                SourceObject = t,
                Symbol = t.Symbol,
                HighPrice = t.HighPrice,
                LowPrice = t.LowPrice,
                Volume = t.Volume
            }));
        }

        async Task<WebCallResult<IEnumerable<Trade>>> IBaseRestClient.GetRecentTradesAsync(string symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetRecentTradesAsync), nameof(symbol));

            var tradesResult = await ExchangeData.GetTradeHistoryAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!tradesResult)
                return tradesResult.As<IEnumerable<Trade>>(null);

            return tradesResult.As(tradesResult.Data.Select(t => new Trade
            {
                SourceObject = t,
                Symbol = symbol,
                Price = t.Price,
                Quantity = t.Quantity,
                Timestamp = t.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<Kline>>> IBaseRestClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime, DateTime? endTime, int? limit, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetKlinesAsync), nameof(symbol));

            if (startTime.HasValue)
            {
                var interval = GetKlineIntervalFromTimespan(timespan);
                var klines = await ExchangeData.GetHistoricalKlinesAsync(symbol, interval,
                    startTime.Value.Year,
                    interval == KlineInterval.OneDay ? null : (int?)startTime.Value.Month,
                    interval == KlineInterval.OneDay || interval == KlineInterval.OneHour ? null : (int?)startTime.Value.Day, ct: ct).ConfigureAwait(false);
                if (!klines)
                    return klines.As<IEnumerable<Kline>>(null);

                return klines.As(klines.Data.Select(t => new Kline
                {
                    SourceObject = t,
                    OpenPrice = t.OpenPrice,
                    OpenTime = t.OpenTime,
                    ClosePrice = t.ClosePrice,
                    HighPrice = t.HighPrice,
                    LowPrice = t.LowPrice,
                    Volume = t.Volume                    
                }));
            }
            else
            {
                var klines = await ExchangeData.GetKlinesAsync(symbol, GetKlineIntervalFromTimespan(timespan), ct: ct).ConfigureAwait(false);
                if (!klines)
                    return klines.As<IEnumerable<Kline>>(null);

                return klines.As(klines.Data.Select(t => new Kline
                {
                    SourceObject = t,
                    OpenPrice = t.OpenPrice,
                    OpenTime = t.OpenTime,
                    ClosePrice = t.ClosePrice,
                    HighPrice = t.HighPrice,
                    LowPrice = t.LowPrice,
                    Volume = t.Volume
                }));
            }
        }

        async Task<WebCallResult<OrderId>> ISpotClient.PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price, string? accountId, string? clientOrderId, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.PlaceOrderAsync), nameof(symbol));

            var result = await Trading.PlaceOrderAsync(symbol, GetOrderSide(side), GetOrderType(type), TimeInForce.GoodTillCanceled, quantity, price: price, clientOrderId: clientOrderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<OrderId>(null);

            return result.As(new OrderId { SourceObject = result.Data, Id = result.Data.Id });
        }

        async Task<WebCallResult<Order>> IBaseRestClient.GetOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentException(nameof(orderId) + " required for Bittrex " + nameof(ISpotClient.GetOrderAsync), nameof(orderId));

            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetOrderAsync), nameof(symbol));

            var result = await Trading.GetOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<Order>(null);

            return result.As(new Order
            {
                SourceObject = result.Data,
                QuantityFilled = result.Data.QuantityFilled,
                Id = result.Data.Id,
                Price = result.Data.Price ?? 0,
                Quantity = result.Data.Quantity ?? 0,
                Symbol = result.Data.Symbol,
                Timestamp = result.Data.CreateTime,
                Side = result.Data.Side == OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Status = result.Data.Status == OrderStatus.Open ? CommonOrderStatus.Active : result.Data.QuantityFilled == result.Data.Quantity ? CommonOrderStatus.Filled : CommonOrderStatus.Canceled,
                Type = result.Data.Type == OrderType.Market ? CommonOrderType.Market : result.Data.Type == OrderType.Limit ? CommonOrderType.Limit : CommonOrderType.Other
            });
        }

        async Task<WebCallResult<IEnumerable<UserTrade>>> IBaseRestClient.GetOrderTradesAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentException(nameof(orderId) + " required for Bittrex " + nameof(ISpotClient.GetOrderTradesAsync), nameof(orderId))
                    ;
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException(nameof(symbol) + " required for Bittrex " + nameof(ISpotClient.GetOrderTradesAsync), nameof(symbol));

            var result = await Trading.GetUserTradesAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<UserTrade>>(null);

            return result.As(result.Data.Select(t => new UserTrade
            {
                SourceObject = t,
                OrderId = t.OrderId,
                Fee = t.Fee,
                Id = t.Id,
                Price = t.Price,
                Quantity = t.Quantity,
                Symbol = t.Symbol,
                Timestamp = t.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetOpenOrdersAsync(string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Order>>(null);

            return result.As(result.Data.Select(t => new Order
            {
                SourceObject = t,
                QuantityFilled = t.QuantityFilled,
                Id = t.Id,
                Price = t.Price ?? 0,
                Quantity = t.Quantity ?? 0,
                Symbol = t.Symbol,
                Timestamp = t.CreateTime,
                Side = t.Side == OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Status = t.Status == OrderStatus.Open ? CommonOrderStatus.Active : t.QuantityFilled == t.Quantity ? CommonOrderStatus.Filled : CommonOrderStatus.Canceled,
                Type = t.Type == OrderType.Market ? CommonOrderType.Market : t.Type == OrderType.Limit ? CommonOrderType.Limit : CommonOrderType.Other
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetClosedOrdersAsync(string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetClosedOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Order>>(null);

            return result.As(result.Data.Select(t => new Order
            {
                SourceObject = t,
                QuantityFilled = t.QuantityFilled,
                Id = t.Id,
                Price = t.Price ?? 0,
                Quantity = t.Quantity ?? 0,
                Symbol = t.Symbol,
                Timestamp = t.CreateTime,
                Side = t.Side == OrderSide.Buy ? CommonOrderSide.Buy: CommonOrderSide.Sell,
                Status = t.Status == OrderStatus.Open ? CommonOrderStatus.Active: t.QuantityFilled == t.Quantity ? CommonOrderStatus.Filled: CommonOrderStatus.Canceled,
                Type = t.Type == OrderType.Market ? CommonOrderType.Market: t.Type == OrderType.Limit ? CommonOrderType.Limit: CommonOrderType.Other
            }));
        }

        async Task<WebCallResult<OrderId>> IBaseRestClient.CancelOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(orderId))
                throw new ArgumentException(nameof(orderId) + " required for Bittrex " + nameof(ISpotClient.CancelOrderAsync), nameof(orderId));

            var result = await Trading.CancelOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<OrderId>(null);

            return result.As(new OrderId { SourceObject = result.Data, Id = result.Data.Id });
        }

        async Task<WebCallResult<IEnumerable<Balance>>> IBaseRestClient.GetBalancesAsync(string? accountId, CancellationToken ct)
        {
            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Balance>>(null);

            return result.As(result.Data.Select(d => new Balance
            {
                SourceObject = d,
                Asset = d.Asset,
                Available = d.Available,
                Total = d.Total
            }));
        }

        internal void InvokeOrderPlaced(OrderId id)
        {
            OnOrderPlaced?.Invoke(id);
        }

        internal void InvokeOrderCanceled(OrderId id)
        {
            OnOrderCanceled?.Invoke(id);
        }

        private static KlineInterval GetKlineIntervalFromTimespan(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.FromMinutes(1)) return KlineInterval.OneMinute;
            if (timeSpan == TimeSpan.FromMinutes(5)) return KlineInterval.FiveMinutes;
            if (timeSpan == TimeSpan.FromHours(1)) return KlineInterval.OneHour;
            if (timeSpan == TimeSpan.FromDays(1)) return KlineInterval.OneDay;

            throw new ArgumentException("Unsupported timespan for Bittrex Klines, check supported intervals using Bittrex.Net.Objects.KlineInterval");
        }

        private static OrderSide GetOrderSide(CommonOrderSide side)
        {
            if (side == CommonOrderSide.Sell) return Enums.OrderSide.Sell;
            if (side == CommonOrderSide.Buy) return Enums.OrderSide.Buy;

            throw new ArgumentException("Unsupported order side for Bittrex order: " + side);
        }

        private static OrderType GetOrderType(CommonOrderType type)
        {
            if (type == CommonOrderType.Limit) return Enums.OrderType.Limit;
            if (type == CommonOrderType.Market) return Enums.OrderType.Market;

            throw new ArgumentException("Unsupported order type for Bittrex order: " + type);
        }


        /// <inheritdoc />
        public string GetSymbolName(string baseAsset, string quoteAsset) => $"{baseAsset}-{quoteAsset}".ToUpperInvariant();
        #endregion

        /// <summary>
        /// Get url for an endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        internal Uri GetUrl(string endpoint)
        {
            return new Uri(_options.SpotApiOptions.BaseAddress.AppendPath($"v3", endpoint));
        }

        internal Task<WebCallResult<T>> SendRequestAsync<T>(
             Uri uri,
             HttpMethod method,
             CancellationToken cancellationToken,
             Dictionary<string, object>? parameters = null,
             bool signed = false,
             JsonSerializer? deserializer = null,
             bool ignoreRateLimit = false) where T : class
                 => _baseClient.SendRequestAsync<T>(this, uri, method, cancellationToken, parameters, signed, deserializer: deserializer, ignoreRateLimit: ignoreRateLimit);


        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        protected override TimeSyncInfo GetTimeSyncInfo()
            => new TimeSyncInfo(_log, _options.SpotApiOptions.AutoTimestamp, _options.SpotApiOptions.TimestampRecalculationInterval, TimeSyncState);

        /// <inheritdoc />
        public override TimeSpan GetTimeOffset()
            => TimeSyncState.TimeOffset;

        /// <inheritdoc />
        public ISpotClient CommonSpotClient => this;
    }
}
