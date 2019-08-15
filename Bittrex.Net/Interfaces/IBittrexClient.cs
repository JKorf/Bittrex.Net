using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Bittrex client interface
    /// </summary>
    public interface IBittrexClient: IRestClient
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
        WebCallResult<BittrexMarket[]> GetMarkets();

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        Task<WebCallResult<BittrexMarket[]>> GetMarketsAsync();

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        WebCallResult<BittrexCurrency[]> GetCurrencies();

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        Task<WebCallResult<BittrexCurrency[]>> GetCurrenciesAsync();

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        WebCallResult<BittrexPrice> GetTicker(string market);

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        Task<WebCallResult<BittrexPrice>> GetTickerAsync(string market);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        WebCallResult<BittrexMarketSummary> GetMarketSummary(string market);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        Task<WebCallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market);

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        WebCallResult<BittrexMarketSummary[]> GetMarketSummaries();

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        Task<WebCallResult<BittrexMarketSummary[]>> GetMarketSummariesAsync();

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<BittrexOrderBook> GetOrderBook(string market);

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string market);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        WebCallResult<BittrexMarketHistory[]> GetMarketHistory(string market);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        Task<WebCallResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        WebCallResult<BittrexCandle[]> GetCandles(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        Task<WebCallResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        WebCallResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        Task<WebCallResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        WebCallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        Task<WebCallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate);
        
        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        WebCallResult<object> CancelOrder(Guid guid);

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        Task<WebCallResult<object>> CancelOrderAsync(Guid guid);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        WebCallResult<BittrexOpenOrdersOrder[]> GetOpenOrders(string market = null);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<BittrexOpenOrdersOrder[]>> GetOpenOrdersAsync(string market = null);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        WebCallResult<BittrexBalance> GetBalance(string currency);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string currency);

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        WebCallResult<BittrexBalance[]> GetBalances();

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        Task<WebCallResult<BittrexBalance[]>> GetBalancesAsync();

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        WebCallResult<BittrexDepositAddress> GetDepositAddress(string currency);

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        WebCallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        Task<WebCallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        WebCallResult<BittrexAccountOrder> GetOrder(Guid guid);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        Task<WebCallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        WebCallResult<BittrexOrderHistoryOrder[]> GetOrderHistory(string market = null);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        Task<WebCallResult<BittrexOrderHistoryOrder[]>> GetOrderHistoryAsync(string market = null);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        WebCallResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        Task<WebCallResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        WebCallResult<BittrexDeposit[]> GetDepositHistory(string currency = null);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null);
    }
}