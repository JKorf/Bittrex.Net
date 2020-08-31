using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Interface for the Bittrex V3 API client
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
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        WebCallResult<IEnumerable<BittrexSymbolV3>> GetSymbols(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        Task<WebCallResult<IEnumerable<BittrexSymbolV3>>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        WebCallResult<BittrexSymbolV3> GetSymbol(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets information about a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol info</returns>
        Task<WebCallResult<BittrexSymbolV3>> GetSymbolAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets summaries of all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbol summaries</returns>
        WebCallResult<IEnumerable<BittrexSymbolSummaryV3>> GetSymbolSummaries(CancellationToken ct = default);

        /// <summary>
        /// Gets summaries of all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbol summaries</returns>
        Task<WebCallResult<IEnumerable<BittrexSymbolSummaryV3>>> GetSymbolSummariesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets summary of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol summary</returns>
        WebCallResult<BittrexSymbolSummaryV3> GetSymbolSummary(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets summary of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol summary</returns>
        Task<WebCallResult<BittrexSymbolSummaryV3>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The number of results per side for the order book (1, 25 or 500)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol order book</returns>
        WebCallResult<BittrexOrderBookV3> GetOrderBook(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The number of results per side for the order book (1, 25 or 500)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol order book</returns>
        Task<WebCallResult<BittrexOrderBookV3>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the trade history of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol trade list</returns>
        WebCallResult<IEnumerable<Objects.V3.BittrexSymbolTrade>> GetSymbolTrades(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the trade history of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol trade list</returns>
        Task<WebCallResult<IEnumerable<Objects.V3.BittrexSymbolTrade>>> GetSymbolTradesAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the ticker of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol ticker</returns>
        WebCallResult<BittrexTickV3> GetTicker(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the ticker of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol ticker</returns>
        Task<WebCallResult<BittrexTickV3>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets list of tickers for all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol tickers</returns>
        WebCallResult<IEnumerable<BittrexTickV3>> GetTickers(CancellationToken ct = default);

        /// <summary>
        /// Gets list of tickers for all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol tickers</returns>
        Task<WebCallResult<IEnumerable<BittrexTickV3>>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the klines for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol kline</returns>
        WebCallResult<IEnumerable<BittrexKlineV3>> GetKlines(string symbol, KlineInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets the klines for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol klines</returns>
        Task<WebCallResult<IEnumerable<BittrexKlineV3>>> GetKlinesAsync(string symbol, KlineInterval interval, CancellationToken ct = default);

        /// <summary>
        /// Gets historical klines for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="year">The year to get klines for</param>
        /// <param name="month">The month to get klines for</param>
        /// <param name="day">The day to get klines for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol kline</returns>
        WebCallResult<IEnumerable<BittrexKlineV3>> GetHistoricalKlines(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, CancellationToken ct = default);

        /// <summary>
        /// Gets historical klines for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="year">The year to get klines for</param>
        /// <param name="month">The month to get klines for</param>
        /// <param name="day">The day to get klines for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol kline</returns>
        Task<WebCallResult<IEnumerable<BittrexKlineV3>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, CancellationToken ct = default);

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
        /// Get account info
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account info</returns>
        WebCallResult<BittrexAccount> GetAccount(CancellationToken ct = default);

        /// <summary>
        /// Get account info
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account info</returns>
        Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account volume
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account volume</returns>
        WebCallResult<BittrexAccountVolume> GetAccountVolume(CancellationToken ct = default);

        /// <summary>
        /// Get account volume
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account volume</returns>
        Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default);

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
        /// <returns>Balance for currency</returns>
        WebCallResult<BittrexBalanceV3> GetBalance(string currency, CancellationToken ct = default);

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for currency</returns>
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
        /// Gets executions (trades) for a order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve executions for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        WebCallResult<IEnumerable<BittrexExecution>> GetExecutions(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Gets executions (trades) for a order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve executions for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        Task<WebCallResult<IEnumerable<BittrexExecution>>> GetExecutionsAsync(string orderId, CancellationToken ct = default);

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
        /// <param name="useAwards">Whether to use Bittrex awards</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, TimeInForce timeInForce, decimal quantity, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default);

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
        /// <param name="useAwards">Whether to use Bittrex awards</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, TimeInForce timeInForce, decimal? quantity, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default);

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
        WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string? addressTag = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string? addressTag = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of whitelisted address for withdrawals
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawal address</returns>
        WebCallResult<IEnumerable<BittrexWhitelistAddress>> GetWithdrawalWhitelistAddresses(CancellationToken ct = default);

        /// <summary>
        /// Gets a list of whitelisted address for withdrawals
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawal address</returns>
        Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get details on a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        WebCallResult<BittrexConditionalOrder> GetConditionalOrder(string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get details on a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancels a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        WebCallResult<BittrexConditionalOrder> CancelConditionalOrder(string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Cancels a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="startDate">Filter by date</param>
        /// <param name="endDate">Filter by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed conditional orders</returns>
        WebCallResult<IEnumerable<BittrexConditionalOrder>> GetClosedConditionalOrders(string? symbol = null, DateTime? startDate = null,
            DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="startDate">Filter by date</param>
        /// <param name="endDate">Filter by date</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed conditional orders</returns>
        Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Get list op open conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional orders</returns>
        WebCallResult<IEnumerable<BittrexConditionalOrder>> GetOpenConditionalOrders(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get list op open conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional orders</returns>
        Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new conditional order
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
        WebCallResult<BittrexConditionalOrder> PlaceConditionalOrder(string symbol,
            string operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new conditional order
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
            string operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default);
    }
}