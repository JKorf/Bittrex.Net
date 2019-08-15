using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// V3 client interface
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
        /// <returns>Time of the server</returns>
        WebCallResult<DateTime> GetServerTime();

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>Time of the server</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync();

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        WebCallResult<BittrexMarketV3[]> GetMarkets();

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        Task<WebCallResult<BittrexMarketV3[]>> GetMarketsAsync();

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List of markets</returns>
        WebCallResult<BittrexMarketV3> GetMarket(string market);

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market info</returns>
        Task<WebCallResult<BittrexMarketV3>> GetMarketAsync(string market);

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of market summaries</returns>
        WebCallResult<BittrexMarketSummariesV3[]> GetMarketSummaries();

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of market summaries</returns>
        Task<WebCallResult<BittrexMarketSummariesV3[]>> GetMarketSummariesAsync();

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market summary</returns>
        WebCallResult<BittrexMarketSummariesV3> GetMarketSummary(string market);

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market summary</returns>
        Task<WebCallResult<BittrexMarketSummariesV3>> GetMarketSummaryAsync(string market);

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Market order book</returns>
        WebCallResult<BittrexMarketOrderBookV3> GetMarketOrderBook(string market);

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Market order book</returns>
        Task<WebCallResult<BittrexMarketOrderBookV3>> GetMarketOrderBookAsync(string market);

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <returns>Market trade list</returns>
        WebCallResult<BittrexMarketTradeV3[]> GetMarketTrades(string market);

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <returns>Market trade list</returns>
        Task<WebCallResult<BittrexMarketTradeV3[]>> GetMarketTradesAsync(string market);

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <returns>Market ticker</returns>
        WebCallResult<BittrexMarketTickV3> GetMarketTicker(string market);

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <returns>Market ticker</returns>
        Task<WebCallResult<BittrexMarketTickV3>> GetMarketTickerAsync(string market);

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <returns>Market tickers</returns>
        WebCallResult<BittrexMarketTickV3[]> GetMarketTickers();

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <returns>Market tickers</returns>
        Task<WebCallResult<BittrexMarketTickV3[]>> GetMarketTickersAsync();

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns>Market candles</returns>
        WebCallResult<BittrexMarketCandleV3[]> GetMarketCandles(string market, CandleInterval interval);

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns>Market candles</returns>
        Task<WebCallResult<BittrexMarketCandleV3[]>> GetMarketCandlesAsync(string market, CandleInterval interval);

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        WebCallResult<BittrexCurrencyV3[]> GetCurrencies();

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        Task<WebCallResult<BittrexCurrencyV3[]>> GetCurrenciesAsync();

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <returns>Currency info</returns>
        WebCallResult<BittrexCurrencyV3> GetCurrency(string currency);

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <returns>Currency info</returns>
        Task<WebCallResult<BittrexCurrencyV3>> GetCurrencyAsync(string currency);

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <returns>List of balances</returns>
        WebCallResult<BittrexBalanceV3[]> GetBalances();

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <returns>List of balances</returns>
        Task<WebCallResult<BittrexBalanceV3[]>> GetBalancesAsync();

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <returns>Balance for market</returns>
        WebCallResult<BittrexBalanceV3> GetBalance(string currency);

        /// <summary>
        /// Gets current balance for a market
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <returns>Balance for market</returns>
        Task<WebCallResult<BittrexBalanceV3>> GetBalanceAsync(string currency);

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <returns>List of deposit addresses</returns>
        WebCallResult<BittrexDepositAddressV3[]> GetDepositAddresses();

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <returns>List of deposit addresses</returns>
        Task<WebCallResult<BittrexDepositAddressV3[]>> GetDepositAddressesAsync();

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <returns>Deposit addresses</returns>
        WebCallResult<BittrexDepositAddressV3> GetDepositAddress(string currency);

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <returns>Deposit addresses</returns>
        Task<WebCallResult<BittrexDepositAddressV3>> GetDepositAddressAsync(string currency);

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <returns>The deposit address</returns>
        WebCallResult<BittrexDepositAddressV3> RequestDepositAddress(string currency);

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <returns>The deposit address</returns>
        Task<WebCallResult<BittrexDepositAddressV3>> RequestDepositAddressAsync(string currency);

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <returns>List of deposits</returns>
        WebCallResult<BittrexDepositV3[]> GetOpenDeposits(string currency = null);

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<BittrexDepositV3[]>> GetOpenDepositsAsync(string currency = null);

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
        /// <returns>List of deposits</returns>
        WebCallResult<BittrexDepositV3[]> GetClosedDeposits(string currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

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
        /// <returns>List of deposits</returns>
        Task<WebCallResult<BittrexDepositV3[]>> GetClosedDepositsAsync(string currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List of deposits</returns>
        WebCallResult<BittrexDepositV3[]> GetDepositsByTransactionId(string transactionId);

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<BittrexDepositV3[]>> GetDepositsByTransactionIdAsync(string transactionId);

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <returns>Deposit info</returns>
        WebCallResult<BittrexDepositV3> GetDeposit(string depositId);

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <returns>Deposit info</returns>
        Task<WebCallResult<BittrexDepositV3>> GetDepositAsync(string depositId);

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <param name="symbol">Filter the list by symbol</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last order id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first order id of the next page</param>
        /// <returns>List of closed orders</returns>
        WebCallResult<BittrexOrderV3[]> GetClosedOrders(string symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <param name="symbol">Filter the list by symbol</param>
        /// <param name="startDate">Filter the list by date</param>
        /// <param name="endDate">Filter the list by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last order id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first order id of the next page</param>
        /// <returns>List of closed orders</returns>
        Task<WebCallResult<BittrexOrderV3[]>> GetClosedOrdersAsync(string symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <returns>List of open orders</returns>
        WebCallResult<BittrexOrderV3[]> GetOpenOrders(string symbol = null);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<BittrexOrderV3[]>> GetOpenOrdersAsync(string symbol = null);

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <returns>Order info</returns>
        WebCallResult<BittrexOrderV3> GetOrder(string orderId);

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrderV3>> GetOrderAsync(string orderId);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <returns>Order info</returns>
        WebCallResult<BittrexOrderV3> CancelOrder(string orderId);

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <returns>Order info</returns>
        Task<WebCallResult<BittrexOrderV3>> CancelOrderAsync(string orderId);

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
        /// <returns>The order info</returns>
        WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity,  TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string clientOrderId = null);

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
        /// <returns>The order info</returns>
        Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity, TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string clientOrderId = null);

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <returns>List of open withdrawals</returns>
        WebCallResult<BittrexWithdrawalV3[]> GetOpenWithdrawals(string currency = null, WithdrawalStatus? status = null);

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <returns>List of open withdrawals</returns>
        Task<WebCallResult<BittrexWithdrawalV3[]>> GetOpenWithdrawalsAsync(string currency = null, WithdrawalStatus? status = null);

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
        /// <returns>List of closed withdrawals</returns>
        WebCallResult<BittrexWithdrawalV3[]> GetClosedWithdrawals(string currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

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
        /// <returns>List of closed withdrawals</returns>
        Task<WebCallResult<BittrexWithdrawalV3[]>> GetClosedWithdrawalsAsync(string currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null);

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List withdrawals</returns>
        WebCallResult<BittrexWithdrawalV3[]> GetWithdrawalsByTransactionId(string transactionId);

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List withdrawals</returns>
        Task<WebCallResult<BittrexWithdrawalV3[]>> GetWithdrawalsByTransactionIdAsync(string transactionId);

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <returns>Withdrawal info</returns>
        WebCallResult<BittrexWithdrawalV3> GetWithdrawal(string id);

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> GetWithdrawalAsync(string id);

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <returns>Withdrawal info</returns>
        WebCallResult<BittrexWithdrawalV3> CancelWithdrawal(string id);

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> CancelWithdrawalAsync(string id);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <returns>Info about the withdrawal</returns>
        WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string addressTag);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <returns>Info about the withdrawal</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string addressTag);
    }
}