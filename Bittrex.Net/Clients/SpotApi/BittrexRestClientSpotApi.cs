using Bittrex.Net.Enums;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IBittrexRestClientSpotApi" />
    public class BittrexRestClientSpotApi : RestApiClient, IBittrexRestClientSpotApi, ISpotClient
    {
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Api");

        /// <inheritdoc />
        public string ExchangeName => "Bittrex";

        #region Api clients

        /// <inheritdoc />
        public IBittrexRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IBittrexRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBittrexRestClientSpotApiTrading Trading { get; }

        #endregion

        internal BittrexRestClientSpotApi(ILogger logger, HttpClient? httpClient, BittrexRestOptions options) :
            base(logger, httpClient, options.Environment.RestAddress, options, options.SpotOptions)
        {
            Account = new BittrexRestClientSpotApiAccount(this);
            ExchangeData = new BittrexRestClientSpotApiExchangeData(this);
            Trading = new BittrexRestClientSpotApiTrading(this);
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

            var tickerTask = ExchangeData.GetTickerAsync(symbol, ct: ct);
            var summaryTask = ExchangeData.GetSymbolSummaryAsync(symbol, ct: ct);
            await Task.WhenAll(tickerTask, summaryTask).ConfigureAwait(false);

            if (!tickerTask.Result)
                return tickerTask.Result.As<Ticker>(null);

            if (!summaryTask.Result)
                return summaryTask.Result.As<Ticker>(null);

            return tickerTask.Result.As(new Ticker
            {
                SourceObject = tickerTask.Result.Data,
                Symbol = tickerTask.Result.Data.Symbol,
                HighPrice = summaryTask.Result.Data.HighPrice,
                LowPrice = summaryTask.Result.Data.LowPrice,
                Volume = summaryTask.Result.Data.Volume,
                LastPrice = tickerTask.Result.Data.LastPrice
            });
        }

        async Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            var tickerTask = ExchangeData.GetTickersAsync(ct: ct);
            var summaryTask = ExchangeData.GetSymbolSummariesAsync(ct: ct);
            await Task.WhenAll(tickerTask, summaryTask).ConfigureAwait(false);

            if (!tickerTask.Result)
                return tickerTask.Result.As<IEnumerable<Ticker>>(null);

            if (!summaryTask.Result)
                return summaryTask.Result.As<IEnumerable<Ticker>>(null);

            return tickerTask.Result.As(summaryTask.Result.Data.Select(t => new Ticker
            {
                SourceObject = t,
                Symbol = t.Symbol,
                HighPrice = t.HighPrice,
                LowPrice = t.LowPrice,
                Volume = t.Volume,
                LastPrice = tickerTask.Result.Data.SingleOrDefault(ti => ti.Symbol == t.Symbol)?.LastPrice
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
                throw new ArgumentException(nameof(orderId) + " required for Bittrex " + nameof(ISpotClient.GetOrderTradesAsync), nameof(orderId));

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
            return new Uri(BaseAddress.AppendPath($"v3", endpoint));
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(int httpStatusCode, IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, string data)
        {
            var errorData = ValidateJson(data);
            if (!errorData)
                return new ServerError(data);

            if (errorData.Data["code"] == null)
                return new UnknownError("Unknown response from server", data);

            var info = (string)errorData.Data["code"]!;
            if (errorData.Data["detail"] != null)
                info += "; Details: " + (string)errorData.Data["detail"]!;
            if (errorData.Data["data"] != null)
                info += "; Data: " + errorData.Data["data"];

            return new ServerError(info);
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            if (parameters.Any() && parameters.First().Key == string.Empty)
            {
                var stringData = JsonConvert.SerializeObject(parameters.First().Value);
                request.SetContent(stringData, contentType);
            }
            else
            {
                var stringData = JsonConvert.SerializeObject(parameters);
                request.SetContent(stringData, contentType);
            }
        }

        internal Task<WebCallResult<T>> SendRequestAsync<T>(
             Uri uri,
             HttpMethod method,
             CancellationToken cancellationToken,
             Dictionary<string, object>? parameters = null,
             bool signed = false,
             JsonSerializer? deserializer = null,
             bool ignoreRateLimit = false) where T : class
                 => base.SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, deserializer: deserializer, ignoreRatelimit: ignoreRateLimit);

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp), (ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval), _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public ISpotClient CommonSpotClient => this;
    }
}
