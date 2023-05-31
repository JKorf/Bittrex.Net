using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.CommonObjects;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BittrexRestClientSpotApiTrading : IBittrexRestClientSpotApiTrading
    {
        private readonly BittrexRestClientSpotApi _baseClient;

        internal BittrexRestClientSpotApiTrading(BittrexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> GetClosedOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            pageSize?.ValidateIntBetween(nameof(pageSize), 1, 200);

            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexOrder>>(_baseClient.GetUrl("orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexOrder>>(_baseClient.GetUrl("orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await _baseClient.SendRequestAsync<BittrexOrder>(_baseClient.GetUrl($"orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexUserTrade>> GetUserTradeByIdAsync(string tradeId, CancellationToken ct = default)
        {
            tradeId.ValidateNotNull(nameof(tradeId));
            return await _baseClient.SendRequestAsync<BittrexUserTrade>(_baseClient.GetUrl($"executions/{tradeId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetUserTradesAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            symbol?.ValidateBittrexSymbol();

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexUserTrade>>(_baseClient.GetUrl("executions"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetOrderTradesAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexUserTrade>>(_baseClient.GetUrl($"orders/{orderId}/executions"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            var result = await _baseClient.SendRequestAsync<BittrexOrder>(_baseClient.GetUrl($"orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data, Id = result.Data.Id });
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexOrder>>(_baseClient.GetUrl("orders/open/"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> PlaceOrderAsync(string symbol, Enums.OrderSide side, Enums.OrderType type, TimeInForce timeInForce, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            if (quantity != null && quoteQuantity != null || quantity == null && quoteQuantity == null)
                throw new ArgumentException("Specify one of either quantity or quoteQantity");

            string orderType = JsonConvert.SerializeObject(type, new OrderTypeConverter(false));
            if (quoteQuantity != null)
                orderType = type == Enums.OrderType.Limit ? "CEILING_LIMIT" : "CEILING_MARKET";

            var parameters = new Dictionary<string, object>()
            {
                {"marketSymbol", symbol},
                {"direction", JsonConvert.SerializeObject(side, new OrderSideConverter(false))},
                {"type", orderType },
                {"timeInForce",  JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)) }
            };
            parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientOrderId", clientOrderId);
            parameters.AddOptionalParameter("ceiling", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("useAwards", useAwards);

            var result = await _baseClient.SendRequestAsync<BittrexOrder>(_baseClient.GetUrl("orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
                _baseClient.InvokeOrderPlaced(new OrderId { SourceObject = result.Data, Id = result.Data.Id });
            return result;
        }

        #endregion

        #region conditional orders

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = null, CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<BittrexConditionalOrder>(_baseClient.GetUrl($"conditional-orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string orderId, CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<BittrexConditionalOrder>(_baseClient.GetUrl($"conditional-orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 1, 200);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexConditionalOrder>>(_baseClient.GetUrl("conditional-orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexConditionalOrder>>(_baseClient.GetUrl("conditional-orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> PlaceConditionalOrderAsync(
            string symbol,
            ConditionalOrderOperand operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "marketSymbol", symbol },
                { "operand", JsonConvert.SerializeObject(operand, new ConditionalOrderOperandConverter(false)) }
            };

            parameters.AddOptionalParameter("triggerPrice", triggerPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopPercent", trailingStopPercent?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientConditionalOrderId", clientConditionalOrderId);
            parameters.AddOptionalParameter("orderToCreate", orderToCreate);
            parameters.AddOptionalParameter("orderToCancel", orderToCancel);

            return await _baseClient.SendRequestAsync<BittrexConditionalOrder>(_baseClient.GetUrl("conditional-orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region batch

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> PlaceMultipleOrdersAsync(BittrexNewBatchOrder[] orders, CancellationToken ct = default)
        {
            orders.ValidateNotNull(nameof(orders));
            if (!orders!.Any())
                throw new ArgumentException("No orders provided");

            var wrapper = new Dictionary<string, object>();
            var orderParameters = new Dictionary<string, object>[orders!.Length];
            int i = 0;
            foreach (var order in orders)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("resource", "ORDER");
                parameters.Add("operation", "POST");
                var orderParameter = new Dictionary<string, object>()
                {
                    {"marketSymbol", order.Symbol},
                    {"direction", JsonConvert.SerializeObject(order.Side, new OrderSideConverter(false))},
                    {"type", JsonConvert.SerializeObject(order.Type, new OrderTypeConverter(false)) },
                    {"timeInForce",  JsonConvert.SerializeObject(order.TimeInForce, new TimeInForceConverter(false)) }
                };
                orderParameter.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
                orderParameter.AddOptionalParameter("limit", order.Price?.ToString(CultureInfo.InvariantCulture));
                orderParameter.AddOptionalParameter("clientOrderId", order.ClientOrderId);
                orderParameter.AddOptionalParameter("ceiling", order.Ceiling?.ToString(CultureInfo.InvariantCulture));
                orderParameter.AddOptionalParameter("useAwards", order.UseAwards);
                parameters.Add("payload", orderParameter);
                orderParameters[i] = parameters;
                i++;
            }
            wrapper.Add(string.Empty, orderParameters);

            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BatchResultConverter<BittrexOrder>()
                }
            });

            return await _baseClient.SendRequestAsync<IEnumerable<CallResult<BittrexOrder>>>(_baseClient.GetUrl("batch"), HttpMethod.Post, ct, wrapper, signed: true, deserializer: serializer).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> CancelMultipleOrdersAsync(string[] ordersToCancel, CancellationToken ct = default)
        {
            ordersToCancel.ValidateNotNull(nameof(ordersToCancel));
            if (!ordersToCancel!.Any())
                throw new ArgumentException("No orders provided");

            var wrapper = new Dictionary<string, object>();
            var orderParameters = new Dictionary<string, object>[ordersToCancel!.Length];
            int i = 0;
            foreach (var order in ordersToCancel)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("resource", "ORDER");
                parameters.Add("operation", "DELETE");
                var orderParameter = new Dictionary<string, object>()
                {
                    {"id", order},
                };
                parameters.Add("payload", orderParameter);
                orderParameters[i] = parameters;
                i++;
            }
            wrapper.Add(string.Empty, orderParameters);

            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BatchResultConverter<BittrexOrder>()
                }
            });

            return await _baseClient.SendRequestAsync<IEnumerable<CallResult<BittrexOrder>>>(_baseClient.GetUrl("batch"), HttpMethod.Post, ct, wrapper, signed: true, deserializer: serializer).ConfigureAwait(false);
        }
        #endregion

    }
}
