using Bittrex.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects.Models;

namespace Bittrex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Bittrex trading endpoints, placing and mananging orders.
    /// </summary>
    public interface IBittrexRestClientSpotApiTrading
    {
        /// <summary>
        /// Gets a list of closed orders
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders-closed-get" /></para>
        /// </summary>
        /// <param name="symbol">Filter the list by symbol</param>
        /// <param name="startTime">Filter the list by time</param>
        /// <param name="endTime">Filter the list by time</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last order id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first order id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOrder>>> GetClosedOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open orders. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders-open-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOrder>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Gets info on an order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders--orderId--get" /></para>
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrder>> GetOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Gets info on a user trade
        /// <para><a href="https://bittrex.github.io/api/v3#operation--executions--executionId--get" /></para>
        /// </summary>
        /// <param name="tradeId">The id of the trade to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade info</returns>
        Task<WebCallResult<BittrexUserTrade>> GetUserTradeByIdAsync(string tradeId, CancellationToken ct = default);

        /// <summary>
        /// Gets user trades
        /// <para><a href="https://bittrex.github.io/api/v3#operation--executions-get" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="startTime">Filter the list by time</param>
        /// <param name="endTime">Filter the list by time</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last withdrawal id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first withdrawal id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetUserTradesAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets trades for an order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders--orderId--executions-get" /></para>
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetOrderTradesAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancels an order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders--orderId--delete" /></para>
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrder>> CancelOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancels all open orders
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders-open-delete" /></para>
        /// </summary>
        /// <param name="symbol">Only cancel open orders for a specific symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<IEnumerable<BittrexOrder>>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Places an order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--orders-post" /></para>
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="timeInForce">The time in force of the order</param>
        /// <param name="price">The price of the order (limit orders only)</param>
        /// <param name="quoteQuantity">The amount of quote quantity to use</param>
        /// <param name="clientOrderId">Id to track the order by</param>
        /// <param name="useAwards">Option to use Bittrex credits for the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        Task<WebCallResult<BittrexOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, TimeInForce timeInForce, decimal? quantity = null, decimal? price = null, decimal? quoteQuantity = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple orders in a single call
        /// <para><a href="https://bittrex.github.io/api/v3#operation--batch-post" /></para>
        /// </summary>
        /// <param name="orders">Orders to place</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A WebCallResult indicating the result of the call, which contains a collection of CallResults for each of the placed orders</returns>
        Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> PlaceMultipleOrdersAsync(BittrexNewBatchOrder[] orders, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders in a single call
        /// <para><a href="https://bittrex.github.io/api/v3#operation--batch-post" /></para>
        /// </summary>
        /// <param name="ordersToCancel">Orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A WebCallResult indicating the result of the call, which contains a collection of CallResults for each of the canceled orders</returns>
        Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> CancelMultipleOrdersAsync(string[] ordersToCancel, CancellationToken ct = default);


        /// <summary>
        /// Get details on a condtional order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--get" /></para>
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancels a condtional order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--conditional-orders--conditionalOrderId--delete" /></para>
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed conditional orders
        /// <para><a href="https://bittrex.github.io/api/v3#operation--conditional-orders-closed-get" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="startTime">Filter the list by time</param>
        /// <param name="endTime">Filter the list by time</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed conditional orders</returns>
        Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Get list op open conditional orders
        /// <para><a href="https://bittrex.github.io/api/v3#operation--conditional-orders-open-get" /></para>
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional orders</returns>
        Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new conditional order
        /// <para><a href="https://bittrex.github.io/api/v3#operation--conditional-orders-post" /></para>
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="operand">The operand of the order</param>
        /// <param name="orderToCreate">Order to create when condition is triggered</param>
        /// <param name="orderToCancel">Order to cancel when condition is triggered</param>
        /// <param name="triggerPrice">Trigger price</param>
        /// <param name="trailingStopPercent">Trailing stop percent</param>
        /// <param name="clientConditionalOrderId">Client order id for conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Condition order</returns>
        Task<WebCallResult<BittrexConditionalOrder>> PlaceConditionalOrderAsync(
            string symbol,
            ConditionalOrderOperand operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default);
    }
}
