using System;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;

namespace Bittrex.Net.Interfaces
{
    public interface IBittrexClient
    {
        /// <summary>
        /// Set the API key and secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        CallResult<BittrexMarket[]> GetMarkets();

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        Task<CallResult<BittrexMarket[]>> GetMarketsAsync();

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        CallResult<BittrexCurrency[]> GetCurrencies();

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        Task<CallResult<BittrexCurrency[]>> GetCurrenciesAsync();

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        CallResult<BittrexPrice> GetTicker(string market);

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        Task<CallResult<BittrexPrice>> GetTickerAsync(string market);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        CallResult<BittrexMarketSummary> GetMarketSummary(string market);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        Task<CallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market);

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        CallResult<BittrexMarketSummary[]> GetMarketSummaries();

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        Task<CallResult<BittrexMarketSummary[]>> GetMarketSummariesAsync();

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        CallResult<BittrexOrderBook> GetOrderBook(string market);

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<CallResult<BittrexOrderBook>> GetOrderBookAsync(string market);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        CallResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<CallResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        CallResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<CallResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        CallResult<BittrexMarketHistory[]> GetMarketHistory(string market);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        Task<CallResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        CallResult<BittrexCandle[]> GetCandles(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        Task<CallResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        CallResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        Task<CallResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        CallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        Task<CallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate);

        /// <summary>
        /// Places a conditional order. The order will be executed when the condition that is set becomes true.
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="timeInEffect">The time the order stays active</param>
        /// <param name="market">Market the order is for</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate of the order</param>
        /// <param name="conditionType">The type of condition</param>
        /// <param name="target">The target of the condition type</param>
        /// <returns></returns>
        CallResult<BittrexOrderResult> PlaceConditionalOrder(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target);

        /// <summary>
        /// Places a conditional order. The order will be executed when the condition that is set becomes true.
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="timeInEffect">The time the order stays active</param>
        /// <param name="market">Market the order is for</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate of the order</param>
        /// <param name="conditionType">The type of condition</param>
        /// <param name="target">The target of the condition type</param>
        /// <returns></returns>
        Task<CallResult<BittrexOrderResult>> PlaceConditionalOrderAsync(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target);

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        CallResult<object> CancelOrder(Guid guid);

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        Task<CallResult<object>> CancelOrderAsync(Guid guid);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        CallResult<BittrexOpenOrdersOrder[]> GetOpenOrders(string market = null);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        Task<CallResult<BittrexOpenOrdersOrder[]>> GetOpenOrdersAsync(string market = null);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        CallResult<BittrexBalance> GetBalance(string currency);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        Task<CallResult<BittrexBalance>> GetBalanceAsync(string currency);

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        CallResult<BittrexBalance[]> GetBalances();

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        Task<CallResult<BittrexBalance[]>> GetBalancesAsync();

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        CallResult<BittrexDepositAddress> GetDepositAddress(string currency);

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        Task<CallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        CallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        Task<CallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        CallResult<BittrexAccountOrder> GetOrder(Guid guid);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        Task<CallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        CallResult<BittrexOrderHistoryOrder[]> GetOrderHistory(string market = null);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        Task<CallResult<BittrexOrderHistoryOrder[]>> GetOrderHistoryAsync(string market = null);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        CallResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        Task<CallResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        CallResult<BittrexDeposit[]> GetDepositHistory(string currency = null);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        Task<CallResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null);

        /// <summary>
        /// The factory for creating requests. Used for unit testing
        /// </summary>
        IRequestFactory RequestFactory { get; set; }

        /// <summary>
        /// Adds a rate limiter to the client. There are 2 choices, the <see cref="RateLimiterTotal"/> and the <see cref="RateLimiterPerEndpoint"/>.
        /// </summary>
        /// <param name="limiter">The limiter to add</param>
        void AddRateLimiter(IRateLimiter limiter);

        /// <summary>
        /// Removes all rate limiters from this client
        /// </summary>
        void RemoveRateLimiters();

        /// <summary>
        /// Ping to see if the server is reachable
        /// </summary>
        /// <returns>The roundtrip time of the ping request</returns>
        CallResult<long> Ping();

        /// <summary>
        /// Ping to see if the server is reachable
        /// </summary>
        /// <returns>The roundtrip time of the ping request</returns>
        Task<CallResult<long>> PingAsync();

        void Dispose();
    }
}