using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bittrex.Net.Converters;
using Bittrex.Net.Converters.V3;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net
{
    public class BittrexClientV3: RestClient
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();
        private static BittrexClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// NOTE: The V3 API is in open beta. Errors might happen. If so, please report them on http://github.com/bittrex.net
        /// </summary>
        public BittrexClientV3() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// NOTE: The V3 API is in open beta. Errors might happen. If so, please report them on http://github.com/bittrex.net
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
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public WebCallResult<DateTime> GetServerTime() => GetServerTimeAsync().Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
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
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarketV3> GetMarket(string market) => GetMarketAsync(market).Result;

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <returns>Market info</returns>
        public async Task<WebCallResult<BittrexMarketV3>> GetMarketAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketV3>(GetUrl("markets/" + market)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarketSummariesV3[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <returns>List of market info</returns>
        public async Task<WebCallResult<BittrexMarketSummariesV3[]>> GetMarketSummariesAsync()
        {
            return await ExecuteRequest<BittrexMarketSummariesV3[]>(GetUrl("markets/summaries")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketSummariesV3> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketSummariesV3>> GetMarketSummaryAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketSummariesV3>(GetUrl($"markets/{market}/summary")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketOrderBookV3> GetMarketOrderBook(string market) => GetMarketOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketOrderBookV3>> GetMarketOrderBookAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketOrderBookV3>(GetUrl($"markets/{market}/orderbook")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketTradeV3[]> GetMarketTrades(string market) => GetMarketTradesAsync(market).Result;

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketTradeV3[]>> GetMarketTradesAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketTradeV3[]>(GetUrl($"markets/{market}/trades")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketTickV3> GetMarketTicker(string market) => GetMarketTickerAsync(market).Result;

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketTickV3>> GetMarketTickerAsync(string market)
        {
            return await ExecuteRequest<BittrexMarketTickV3>(GetUrl($"markets/{market}/ticker")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketCandleV3[]> GetMarketCandles(string market, CandleInterval interval) => GetMarketCandlesAsync(market, interval).Result;

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <returns>Market summary</returns>
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
        /// <returns>Currency info</returns>
        public WebCallResult<BittrexCurrencyV3> GetCurrency(string currency) => GetCurrencyAsync(currency).Result;

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <returns>Currency info</returns>
        public async Task<WebCallResult<BittrexCurrencyV3>> GetCurrencyAsync(string currency)
        {
            return await ExecuteRequest<BittrexCurrencyV3>(GetUrl($"currencies/{currency}")).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <returns>Market summary</returns>
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
        /// Gets current balance for a market
        /// </summary>
        /// <returns>Balance for market</returns>
        public WebCallResult<BittrexBalanceV3> GetBalance(string currency) => GetBalanceAsync(currency).Result;

        /// <summary>
        /// Gets current balance for a market
        /// </summary>
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
        /// <returns>Deposit addresses</returns>
        public WebCallResult<BittrexDepositAddressV3> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <returns>Deposit addresses</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3>> GetDepositAddressAsync(string currency)
        {
            return await ExecuteRequest<BittrexDepositAddressV3>(GetUrl($"addresses/{currency}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
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
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDepositV3[]> GetOpenDeposits(string market = null) => GetOpenDepositsAsync(market).Result;

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDepositV3[]>> GetOpenDepositsAsync(string market = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", market);

            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl("deposits/open"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of closed deposits
        /// </summary>
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDepositV3[]> GetClosedDeposits(string market = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null) => GetClosedDepositsAsync(market, status, startDate, endDate).Result;

        /// <summary>
        /// Gets list of closed deposits
        /// </summary>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDepositV3[]>> GetClosedDepositsAsync(string market = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", market);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)): null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl("deposits/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDepositV3[]> GetDepositsByTransactionId(string transactionId) => GetDepositsByTransactionIdAsync(transactionId).Result;

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDepositV3[]>> GetDepositsByTransactionIdAsync(string transactionId)
        {
            return await ExecuteRequest<BittrexDepositV3[]>(GetUrl($"deposits/ByTxId/{transactionId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <returns>Deposit info</returns>
        public WebCallResult<BittrexDepositV3> GetDeposit(string depositId) => GetDepositAsync(depositId).Result;

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <returns>Deposit info</returns>
        public async Task<WebCallResult<BittrexDepositV3>> GetDepositAsync(string depositId)
        {
            return await ExecuteRequest<BittrexDepositV3>(GetUrl($"deposits/{depositId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <returns>List of closed orders</returns>
        public WebCallResult<BittrexOrderV3[]> GetClosedOrders(string symbol = null, DateTime? startDate = null, DateTime? endDate = null) => GetClosedOrdersAsync(symbol, startDate, endDate).Result;

        /// <summary>
        /// Gets a list of closed orders
        /// </summary>
        /// <returns>List of closed orders</returns>
        public async Task<WebCallResult<BittrexOrderV3[]>> GetClosedOrdersAsync(string symbol = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            return await ExecuteRequest<BittrexOrderV3[]>(GetUrl("orders/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <returns>List of open orders</returns>
        public WebCallResult<BittrexOrderV3[]> GetOpenOrders(string symbol = null) => GetOpenOrdersAsync(symbol).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3[]>> GetOpenOrdersAsync(string symbol = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await ExecuteRequest<BittrexOrderV3[]>(GetUrl("orders/open"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> GetOrder(string orderId) => GetOrderAsync(orderId).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> GetOrderAsync(string orderId)
        {
            return await ExecuteRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> CancelOrder(string orderId) => CancelOrderAsync(orderId).Result;

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<BittrexOrderV3>> CancelOrderAsync(string orderId)
        {
            return await ExecuteRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), method: Constants.DeleteMethod, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity,  TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string clientOrderId = null) => PlaceOrderAsync(symbol, direction, type, quantity, timeInForce, limit, ceiling, clientOrderId).Result;

        /// <summary>
        /// Places an order
        /// </summary>
        /// <returns>List of open orders</returns>
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
        /// <returns>List of open withdrawals</returns>
        public WebCallResult<BittrexWithdrawalV3[]> GetOpenWithdrawals() => GetOpenWithdrawalsAsync().Result;

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <returns>List of open withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetOpenWithdrawalsAsync()
        {
            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/open"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of closed withdrawals
        /// </summary>
        /// <returns>List of closed withdrawals</returns>
        public WebCallResult<BittrexWithdrawalV3[]> GetClosedWithdrawals(string symbol = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null) => GetClosedWithdrawalsAsync(symbol, status, startDate, endDate).Result;

        /// <summary>
        /// Gets a list of closed withdrawals
        /// </summary>
        /// <returns>List of closed withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetClosedWithdrawalsAsync(string symbol = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", symbol);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/closed"), parameters: parameters, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <returns>List withdrawals</returns>
        public WebCallResult<BittrexWithdrawalV3[]> GetWithdrawalsByTransactionId(string transactionId) => GetWithdrawalsByTransactionIdAsync(transactionId).Result;

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <returns>List withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3[]>> GetWithdrawalsByTransactionIdAsync(string transactionId)
        {
            return await ExecuteRequest<BittrexWithdrawalV3[]>(GetUrl($"withdrawals/ByTxId/{transactionId}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> GetWithdrawal(string id) => GetWithdrawalAsync(id).Result;

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> GetWithdrawalAsync(string id)
        {
            return await ExecuteRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> CancelWithdrawal(string id) => CancelWithdrawalAsync(id).Result;

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> CancelWithdrawalAsync(string id)
        {
            return await ExecuteRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), method:Constants.DeleteMethod, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string addressTag) => WithdrawAsync(currency, quantity, address, addressTag).Result;

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <returns>Withdrawal info</returns>
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
