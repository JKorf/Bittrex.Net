using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Interface for the Bittrex client
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        WebCallResult<IEnumerable<BittrexMarket>> GetMarkets(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        Task<WebCallResult<IEnumerable<BittrexMarket>>> GetMarketsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        WebCallResult<IEnumerable<BittrexCurrency>> GetCurrencies(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        Task<WebCallResult<IEnumerable<BittrexCurrency>>> GetCurrenciesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        WebCallResult<BittrexPrice> GetTicker(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        Task<WebCallResult<BittrexPrice>> GetTickerAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List with single entry containing info for the market</returns>
        WebCallResult<BittrexMarketSummary> GetMarketSummary(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List with single entry containing info for the market</returns>
        Task<WebCallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of summaries for all markets</returns>
        WebCallResult<IEnumerable<BittrexMarketSummary>> GetMarketSummaries(CancellationToken ct = default);

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of summaries for all markets</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketSummary>>> GetMarketSummariesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<BittrexOrderBook> GetOrderBook(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<IEnumerable<BittrexOrderBookEntry>> GetBuyOrderBook(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<IEnumerable<BittrexOrderBookEntry>>> GetBuyOrderBookAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        WebCallResult<IEnumerable<BittrexOrderBookEntry>> GetSellOrderBook(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the market</returns>
        Task<WebCallResult<IEnumerable<BittrexOrderBookEntry>>> GetSellOrderBookAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of trade aggregations</returns>
        WebCallResult<IEnumerable<BittrexMarketHistory>> GetMarketHistory(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of trade aggregations</returns>
        Task<WebCallResult<IEnumerable<BittrexMarketHistory>>> GetMarketHistoryAsync(string market, CancellationToken ct = default);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of candles</returns>
        WebCallResult<IEnumerable<BittrexCandle>> GetCandles(string market, TickInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of candles</returns>
        Task<WebCallResult<IEnumerable<BittrexCandle>>> GetCandlesAsync(string market, TickInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of candles</returns>
        WebCallResult<IEnumerable<BittrexCandle>> GetLatestCandle(string market, TickInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of candles</returns>
        Task<WebCallResult<IEnumerable<BittrexCandle>>> GetLatestCandleAsync(string market, TickInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        WebCallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate, CancellationToken ct = default);

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate, CancellationToken ct = default);

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        WebCallResult<object> CancelOrder(Guid guid, CancellationToken ct = default);

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> CancelOrderAsync(Guid guid, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        WebCallResult<IEnumerable<BittrexOpenOrdersOrder>> GetOpenOrders(string? market = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOpenOrdersOrder>>> GetOpenOrdersAsync(string? market = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The balance of the currency</returns>
        WebCallResult<BittrexBalance> GetBalance(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The balance of the currency</returns>
        Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        WebCallResult<IEnumerable<BittrexBalance>> GetBalances(CancellationToken ct = default);

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address of the currency</returns>
        WebCallResult<BittrexDepositAddress> GetDepositAddress(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address of the currency</returns>
        Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Guid of the withdrawal</returns>
        WebCallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string? paymentId = null, CancellationToken ct = default);

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Guid of the withdrawal</returns>
        Task<WebCallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string? paymentId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The requested order</returns>
        WebCallResult<BittrexAccountOrder> GetOrder(Guid guid, CancellationToken ct = default);

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The requested order</returns>
        Task<WebCallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid, CancellationToken ct = default);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of orders</returns>
        WebCallResult<IEnumerable<BittrexOrderHistoryOrder>> GetOrderHistory(string? market = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of orders</returns>
        Task<WebCallResult<IEnumerable<BittrexOrderHistoryOrder>>> GetOrderHistoryAsync(string? market = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdrawals</returns>
        WebCallResult<IEnumerable<BittrexWithdrawal>> GetWithdrawalHistory(string? currency = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalHistoryAsync(string? currency = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        WebCallResult<IEnumerable<BittrexDeposit>> GetDepositHistory(string? currency = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositHistoryAsync(string? currency = null, CancellationToken ct = default);
    }
}