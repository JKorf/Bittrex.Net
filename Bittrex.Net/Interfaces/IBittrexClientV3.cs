using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Interface for the Bittrex v3 client
    /// </summary>
    public interface IBittrexClientV3: IRestClient
    {
        /// <summary>
        /// Set the API key and secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Time of the server</returns>
        WebCallResult<DateTime> GetServerTime(CancellationToken ct = default);

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Time of the server</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        WebCallResult<IEnumerable<BittrexMarketV3>> GetMarkets(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketV3>>> GetMarketsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        WebCallResult<BittrexMarketV3> GetMarket(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market info</returns>
        Task<WebCallResult<BittrexMarketV3>> GetMarketAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of market summaries</returns>
        WebCallResult<IEnumerable<BittrexMarketSummariesV3>> GetMarketSummaries(CancellationToken ct = default);

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of market summaries</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketSummariesV3>>> GetMarketSummariesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market summary</returns>
        WebCallResult<BittrexMarketSummariesV3> GetMarketSummary(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market summary</returns>
        Task<WebCallResult<BittrexMarketSummariesV3>> GetMarketSummaryAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market order book</returns>
        WebCallResult<BittrexMarketOrderBookV3> GetMarketOrderBook(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market order book</returns>
        Task<WebCallResult<BittrexMarketOrderBookV3>> GetMarketOrderBookAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market trade list</returns>
        WebCallResult<IEnumerable<BittrexMarketTradeV3>> GetMarketTrades(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market trade list</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketTradeV3>>> GetMarketTradesAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market ticker</returns>
        WebCallResult<BittrexMarketTickV3> GetMarketTicker(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market ticker</returns>
        Task<WebCallResult<BittrexMarketTickV3>> GetMarketTickerAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market tickers</returns>
        WebCallResult<IEnumerable<BittrexMarketTickV3>> GetMarketTickers(CancellationToken ct = default);

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market tickers</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketTickV3>>> GetMarketTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market candles</returns>
        WebCallResult<IEnumerable<BittrexMarketCandleV3>> GetMarketCandles(string market, CandleInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market candles</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketCandleV3>>> GetMarketCandlesAsync(string market, CandleInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        WebCallResult<IEnumerable<BittrexCurrencyV3>> GetCurrencies(CancellationToken ct = default);

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        Task<WebCallResult<IEnumerable<BittrexCurrencyV3>>> GetCurrenciesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Currency info</returns>
        WebCallResult<BittrexCurrencyV3> GetCurrency(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Currency info</returns>
        Task<WebCallResult<BittrexCurrencyV3>> GetCurrencyAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        WebCallResult<IEnumerable<BittrexBalanceV3>> GetBalances(CancellationToken ct = default);

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        Task<WebCallResult<IEnumerable<BittrexBalanceV3>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for market</returns>
        WebCallResult<BittrexBalanceV3> GetBalance(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets current balance for a market
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for market</returns>
        Task<WebCallResult<BittrexBalanceV3>> GetBalanceAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposit addresses</returns>
        WebCallResult<IEnumerable<BittrexDepositAddressV3>> GetDepositAddresses(CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposit addresses</returns>
        Task<WebCallResult<IEnumerable<BittrexDepositAddressV3>>> GetDepositAddressesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit addresses</returns>
        WebCallResult<BittrexDepositAddressV3> GetDepositAddress(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit addresses</returns>
        Task<WebCallResult<BittrexDepositAddressV3>> GetDepositAddressAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address</returns>
        WebCallResult<BittrexDepositAddressV3> RequestDepositAddress(string currency, CancellationToken ct = default);

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address</returns>
        Task<WebCallResult<BittrexDepositAddressV3>> RequestDepositAddressAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        WebCallResult<IEnumerable<BittrexDepositV3>> GetOpenDeposits(string? currency = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetOpenDepositsAsync(string? currency = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of closed deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="status">Filter the list by status of the deposit</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last deposit id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first deposit id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        WebCallResult<IEnumerable<BittrexDepositV3>> GetClosedDeposits(string? currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of closed deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="status">Filter the list by status of the deposit</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last deposit id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first deposit id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetClosedDepositsAsync(string? currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        WebCallResult<IEnumerable<BittrexDepositV3>> GetDepositsByTransactionId(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit info</returns>
        WebCallResult<BittrexDepositV3> GetDeposit(string depositId, CancellationToken ct = default);

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit info</returns>
        Task<WebCallResult<BittrexDepositV3>> GetDepositAsync(string depositId, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <param name="symbol">Filter the list by symbol</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last order id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first order id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed orders</returns>
        WebCallResult<IEnumerable<BittrexOrderV3>> GetClosedOrders(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <param name="symbol">Filter the list by symbol</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last order id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first order id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOrderV3>>> GetClosedOrdersAsync(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        WebCallResult<IEnumerable<BittrexOrderV3>> GetOpenOrders(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOrderV3>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        WebCallResult<BittrexOrderV3> GetOrder(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrderV3>> GetOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        WebCallResult<BittrexOrderV3> CancelOrder(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrderV3>> CancelOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="direction">The direction of the order</param>
        /// <param name="type">The type of order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="timeInForce">The time in force of the order</param>
        /// <param name="limit">The limit price of the order (limit orders only)</param>
        /// <param name="ceiling">The ceiling price of the order (ceiling orders only)</param>
        /// <param name="clientOrderId">Id to track the order by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity,  TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="direction">The direction of the order</param>
        /// <param name="type">The type of order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="timeInForce">The time in force of the order</param>
        /// <param name="limit">The limit price of the order (limit orders only)</param>
        /// <param name="ceiling">The ceiling price of the order (ceiling orders only)</param>
        /// <param name="clientOrderId">Id to track the order by</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity, TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open withdrawals</returns>
        WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetOpenWithdrawals(string? currency = null, WithdrawalStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetOpenWithdrawalsAsync(string? currency = null, WithdrawalStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startDate">Filter by date</param>
        /// <param name="endDate">Filter by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last withdrawal id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first withdrawal id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed withdrawals</returns>
        WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetClosedWithdrawals(string? currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startDate">Filter by date</param>
        /// <param name="endDate">Filter by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last withdrawal id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first withdrawal id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetClosedWithdrawalsAsync(string? currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawals</returns>
        WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetWithdrawalsByTransactionId(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        WebCallResult<BittrexWithdrawalV3> GetWithdrawal(string id, CancellationToken ct = default);

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> GetWithdrawalAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        WebCallResult<BittrexWithdrawalV3> CancelWithdrawal(string id, CancellationToken ct = default);

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> CancelWithdrawalAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string addressTag, CancellationToken ct = default);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string addressTag, CancellationToken ct = default);
    }
}