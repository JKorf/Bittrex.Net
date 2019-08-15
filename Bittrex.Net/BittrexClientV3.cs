using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bittrex.Net.Converters;
using Bittrex.Net.Converters.V3;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the V3 API
    /// NOTE: The V3 API is in open beta. Errors might happen. If so, please report them on https://github.com/jkorf/bittrex.net
    /// </summary>
    public class BittrexClientV3: RestClient, IBittrexClientV3
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();
        private static BittrexClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// NOTE: The V3 API is in open beta. Errors might happen. If so, please report them on https://github.com/jkorf/bittrex.net
        /// </summary>
        public BittrexClientV3() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// NOTE: The V3 API is in open beta. Errors might happen. If so, please report them on https://github.com/jkorf/bittrex.net
        /// </summary>
        public BittrexClientV3(BittrexClientOptions options) : base(options, options.ApiCredentials == null ? null : new BittrexAuthenticationProviderV3(options.ApiCredentials))
        {
            Configure(options);
        }
        #endregion

        #region methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(BittrexClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set the API key and secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new BittrexAuthenticationProviderV3(new ApiCredentials(apiKey, apiSecret)));
        }

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>Time of the server</returns>
        public WebCallResult<DateTime> GetServerTime() => GetServerTimeAsync().Result;

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <returns>Time of the server</returns>
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync()
        {
            var result = await ExecuteRequest<BittrexServerTimeV3>(GetUrl("ping")).ConfigureAwait(false);
            return new WebCallResult<DateTime>(result.ResponseStatusCode, result.ResponseHeaders, result.Data?.ServerTime ?? default(DateTime), result.Error);
        }


        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarketV3[]> GetMarkets() => GetMarketsAsync().Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public async Task<WebCallResult<BittrexMarketV3[]>> GetMarketsAsync()
        {
            return await ExecuteRequest<BittrexMarketV3[]>(GetUrl("markets")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarketV3> GetMarket(string market) => GetMarketAsync(market).Result;

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market info</returns>
        public async Task<WebCallResult<BittrexMarketV3>> GetMarketAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketV3>(GetUrl("markets/" + market)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of market summaries</returns>
        public WebCallResult<BittrexMarketSummariesV3[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of market summaries</returns>
        public async Task<WebCallResult<BittrexMarketSummariesV3[]>> GetMarketSummariesAsync()
        {
            return await ExecuteRequest<BittrexMarketSummariesV3[]>(GetUrl("markets/summaries")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketSummariesV3> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketSummariesV3>> GetMarketSummaryAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketSummariesV3>(GetUrl($"markets/{market}/summary")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Market order book</returns>
        public WebCallResult<BittrexMarketOrderBookV3> GetMarketOrderBook(string market) => GetMarketOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Market order book</returns>
        public async Task<WebCallResult<BittrexMarketOrderBookV3>> GetMarketOrderBookAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketOrderBookV3>(GetUrl($"markets/{market}/orderbook")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <returns>Market trade list</returns>
        public WebCallResult<BittrexMarketTradeV3[]> GetMarketTrades(string market) => GetMarketTradesAsync(market).Result;

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <returns>Market trade list</returns>
        public async Task<WebCallResult<BittrexMarketTradeV3[]>> GetMarketTradesAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketTradeV3[]>(GetUrl($"markets/{market}/trades")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <returns>Market ticker</returns>
        public WebCallResult<BittrexMarketTickV3> GetMarketTicker(string market) => GetMarketTickerAsync(market).Result;

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <returns>Market ticker</returns>
        public async Task<WebCallResult<BittrexMarketTickV3>> GetMarketTickerAsync(string market)
        {
            var result = await ExecuteRequest<BittrexMarketTickV3>(GetUrl($"markets/{market}/ticker")).ConfigureAwait(false);
            if (result.Success)
                result.Data.Symbol = market;
            return result;
        }

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <returns>Market tickers</returns>
        public WebCallResult<BittrexMarketTickV3[]> GetMarketTickers() => GetMarketTickersAsync().Result;

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <returns>Market tickers</returns>
        public async Task<WebCallResult<BittrexMarketTickV3[]>> GetMarketTickersAsync()
        {
            return await ExecuteRequest<BittrexMarketTickV3[]>(GetUrl("markets/tickers")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns>Market candles</returns>
        public WebCallResult<BittrexMarketCandleV3[]> GetMarketCandles(string market, CandleInterval interval) => GetMarketCandlesAsync(market, interval).Result;

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <returns>Market candles</returns>
        public async Task<WebCallResult<BittrexMarketCandleV3[]>> GetMarketCandlesAsync(string market, CandleInterval interval)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"candleInterval", JsonConvert.SerializeObject(interval, new CandleIntervalConverter(false))}
            };

            return await ExecuteRequest<BittrexMarketCandleV3[]>(GetUrl($"markets/{market}/candles"), parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public WebCallResult<BittrexCurrencyV3[]> GetCurrencies() => GetCurrenciesAsync().Result;

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public async Task<WebCallResult<BittrexCurrencyV3[]>> GetCurrenciesAsync()
        {
            return await ExecuteRequest<BittrexCurrencyV3[]>(GetUrl("currencies")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <returns>Currency info</returns>
        public WebCallResult<BittrexCurrencyV3> GetCurrency(string currency) => GetCurrencyAsync(currency).Result;

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <returns>Currency info</returns>
        public async Task<WebCallResult<BittrexCurrencyV3>> GetCurrencyAsync(string currency)
        {
            return await ExecuteRequest<BittrexCurrencyV3>(GetUrl($"currencies/{currency}")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <returns>List of balances</returns>
        public WebCallResult<BittrexBalanceV3[]> GetBalances() => GetBalancesAsync().Result;

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <returns>List of balances</returns>
        public async Task<WebCallResult<BittrexBalanceV3[]>> GetBalancesAsync()
        {
            return await ExecuteRequest<BittrexBalanceV3[]>(GetUrl("balances"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <returns>Balance for market</returns>
        public WebCallResult<BittrexBalanceV3> GetBalance(string currency) => GetBalanceAsync(currency).Result;

        /// <summary>
        /// Gets current balance for a market
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <returns>Balance for market</returns>
        public async Task<WebCallResult<BittrexBalanceV3>> GetBalanceAsync(string currency)
        {
            return await ExecuteRequest<BittrexBalanceV3>(GetUrl($"balances/{currency}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <returns>List of deposit addresses</returns>
        public WebCallResult<BittrexDepositAddressV3[]> GetDepositAddresses() => GetDepositAddressesAsync().Result;

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <returns>List of deposit addresses</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3[]>> GetDepositAddressesAsync()
        {
            return await ExecuteRequest<BittrexDepositAddressV3[]>(GetUrl("addresses"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <returns>Deposit addresses</returns>
        public WebCallResult<BittrexDepositAddressV3> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <returns>Deposit addresses</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3>> GetDepositAddressAsync(string currency)
        {
            return await ExecuteRequest<BittrexDepositAddressV3>(GetUrl($"addresses/{currency}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <returns>The deposit address</returns>
        public WebCallResult<BittrexDepositAddressV3> RequestDepositAddress(string currency) => RequestDepositAddressAsync(currency).Result;

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <returns>The deposit address</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3>> RequestDepositAddressAsync(string currency)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency }
            };

            return await ExecuteRequest<BittrexDepositAddressV3>(GetUrl("addresses"), method: Constants.PostMethod, parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDepositV3[]> GetOpenDeposits(string currency = null) => GetOpenDepositsAsync(currency).Result;

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDepositV3[]>> GetOpenDepositsAsync(string currency = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);

            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl("deposits/open"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

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
        public WebCallResult<BittrexDepositV3[]> GetClosedDeposits(string currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null) => GetClosedDepositsAsync(currency, status, startDate, endDate, pageSize, nextPageToken, previousPageToken).Result;

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
        public async Task<WebCallResult<BittrexDepositV3[]>> GetClosedDepositsAsync(string currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null)
        {
            if (nextPageToken != null && previousPageToken != null)
                return WebCallResult<BittrexDepositV3[]>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<BittrexDepositV3[]>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)): null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl("deposits/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDepositV3[]> GetDepositsByTransactionId(string transactionId) => GetDepositsByTransactionIdAsync(transactionId).Result;

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDepositV3[]>> GetDepositsByTransactionIdAsync(string transactionId)
        {
            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl($"deposits/ByTxId/{transactionId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <returns>Deposit info</returns>
        public WebCallResult<BittrexDepositV3> GetDeposit(string depositId) => GetDepositAsync(depositId).Result;

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <returns>Deposit info</returns>
        public async Task<WebCallResult<BittrexDepositV3>> GetDepositAsync(string depositId)
        {
            return await ExecuteRequest<BittrexDepositV3>(GetUrl($"deposits/{depositId}"), signed: true).ConfigureAwait(false);
        }

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
        public WebCallResult<BittrexOrderV3[]> GetClosedOrders(string symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null) => GetClosedOrdersAsync(symbol, startDate, endDate, pageSize, nextPageToken, previousPageToken).Result;

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
        public async Task<WebCallResult<BittrexOrderV3[]>> GetClosedOrdersAsync(string symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null)
        {
            if (nextPageToken != null && previousPageToken != null)
                return WebCallResult<BittrexOrderV3[]>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<BittrexOrderV3[]>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await ExecuteRequest<BittrexOrderV3[]>(GetUrl("orders/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<BittrexOrderV3[]> GetOpenOrders(string symbol = null) => GetOpenOrdersAsync(symbol).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<BittrexOrderV3[]>> GetOpenOrdersAsync(string symbol = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await ExecuteRequest<BittrexOrderV3[]>(GetUrl("orders/open"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> GetOrder(string orderId) => GetOrderAsync(orderId).Result;

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> GetOrderAsync(string orderId)
        {
            return await ExecuteRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> CancelOrder(string orderId) => CancelOrderAsync(orderId).Result;

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> CancelOrderAsync(string orderId)
        {
            return await ExecuteRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), method: Constants.DeleteMethod, signed: true).ConfigureAwait(false);
        }

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
        public WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity,  TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string clientOrderId = null) => PlaceOrderAsync(symbol, direction, type, quantity, timeInForce, limit, ceiling, clientOrderId).Result;

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
        public async Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity, TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string clientOrderId = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"marketSymbol", symbol},
                {"direction", JsonConvert.SerializeObject(direction, new OrderSideConverter(false))},
                {"type", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"timeInForce",  JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)) }
            };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientOrderId", clientOrderId);
            parameters.AddOptionalParameter("ceiling", ceiling?.ToString(CultureInfo.InvariantCulture));

            return await ExecuteRequest<BittrexOrderV3>(GetUrl("orders"), method: Constants.PostMethod, parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <returns>List of open withdrawals</returns>
        public WebCallResult<BittrexWithdrawalV3[]> GetOpenWithdrawals(string currency = null, WithdrawalStatus? status = null) => GetOpenWithdrawalsAsync(currency, status).Result;

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <returns>List of open withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetOpenWithdrawalsAsync(string currency = null, WithdrawalStatus? status = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status);

            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/open"), signed: true).ConfigureAwait(false);
        }

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
        public WebCallResult<BittrexWithdrawalV3[]> GetClosedWithdrawals(string currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null) => GetClosedWithdrawalsAsync(currency, status, startDate, endDate, pageSize, nextPageToken, previousPageToken).Result;

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
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetClosedWithdrawalsAsync(string currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string nextPageToken = null, string previousPageToken = null)
        {
            if (nextPageToken != null && previousPageToken != null)
                return WebCallResult<BittrexWithdrawalV3[]>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<BittrexWithdrawalV3[]>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List withdrawals</returns>
        public WebCallResult<BittrexWithdrawalV3[]> GetWithdrawalsByTransactionId(string transactionId) => GetWithdrawalsByTransactionIdAsync(transactionId).Result;

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <returns>List withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetWithdrawalsByTransactionIdAsync(string transactionId)
        {
            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/ByTxId/{transactionId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> GetWithdrawal(string id) => GetWithdrawalAsync(id).Result;

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> GetWithdrawalAsync(string id)
        {
            return await ExecuteRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> CancelWithdrawal(string id) => CancelWithdrawalAsync(id).Result;

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> CancelWithdrawalAsync(string id)
        {
            return await ExecuteRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), method:Constants.DeleteMethod, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <returns>Info about the withdrawal</returns>
        public WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string addressTag) => WithdrawAsync(currency, quantity, address, addressTag).Result;

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <returns>Info about the withdrawal</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string addressTag)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency},
                { "quantity", quantity},
                { "cryptoAddress", address},
                { "cryptoAddressTag", addressTag},
            };

            return await ExecuteRequest<BittrexWithdrawalV3>(GetUrl("withdrawals"), method: Constants.PostMethod, parameters: parameters, signed: true).ConfigureAwait(false);
        }

        
        protected override Error ParseErrorResponse(JToken data)
        {
            if (data["code"] == null)
                return new UnknownError("Unknown response from server: " + data);

            string info = (string)data["code"];
            if (data["detail"] != null)
                info += "; Details: " + (string) data["detail"];
            if (data["data"] != null)
                info += "; Data: " + data["data"];

            return new ServerError(info);
        }

        protected Uri GetUrl(string endpoint)
        {
            return new Uri($"{BaseAddress}/v3/{endpoint}");
        }
        
        private void Configure(BittrexClientOptions options)
        {
            if (options.ApiCredentials != null)
                SetAuthenticationProvider(new BittrexAuthenticationProviderV3(options.ApiCredentials));
        }
        #endregion
    }
}
