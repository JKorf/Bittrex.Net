using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Newtonsoft.Json;
using Bittrex.Net.Converters;
using Bittrex.Net.Interfaces;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex Rest API
    /// </summary>
    public class BittrexClient: RestClient, IBittrexClient
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();
        private static BittrexClientOptions DefaultOptions => defaultOptions.Copy();

        private const string Api = "api";
        private const string ApiVersion = "1.1";
        private const string ApiVersion2 = "2.0";
        private string baseAddressV2;

        private const string MarketsEndpoint = "public/getmarkets";
        private const string CurrenciesEndpoint = "public/getcurrencies";
        private const string TickerEndpoint = "public/getticker";
        private const string MarketSummariesEndpoint = "public/getmarketsummaries";
        private const string MarketSummaryEndpoint = "public/getmarketsummary";
        private const string OrderBookEndpoint = "public/getorderbook";
        private const string MarketHistoryEndpoint = "public/getmarkethistory";
        private const string CandleEndpoint = "pub/market/GetTicks";
        private const string LatestCandleEndpoint = "pub/market/GetLatestTick";

        private const string BuyLimitEndpoint = "market/buylimit";
        private const string SellLimitEndpoint = "market/selllimit";
        private const string CancelEndpoint = "market/cancel";
        private const string OpenOrdersEndpoint = "market/getopenorders";
        private const string SellV2Endpoint = "key/market/TradeSell";
        private const string BuyV2Endpoint = "key/market/TradeBuy";

        private const string BalanceEndpoint = "account/getbalance";
        private const string BalancesEndpoint = "account/getbalances";
        private const string DepositAddressEndpoint = "account/getdepositaddress";
        private const string WithdrawEndpoint = "account/withdraw";
        private const string OrderEndpoint = "account/getorder";
        private const string OrderHistoryEndpoint = "account/getorderhistory";
        private const string WithdrawalHistoryEndpoint = "account/getwithdrawalhistory";
        private const string DepositHistoryEndpoint = "account/getdeposithistory";
        #endregion

        #region properties
        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// </summary>
        public BittrexClient(): this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the BittrexClient with the provided options
        /// </summary>
        public BittrexClient(BittrexClientOptions options): base(options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            Configure(options);
        }
        #endregion

        #region methods
        #region public
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
            SetAuthenticationProvider(new BittrexAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarket[]> GetMarkets() => GetMarketsAsync().Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public async Task<WebCallResult<BittrexMarket[]>> GetMarketsAsync()
        {
            return await Execute<BittrexMarket[]>(GetUrl(MarketsEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public WebCallResult<BittrexCurrency[]> GetCurrencies() => GetCurrenciesAsync().Result;

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public async Task<WebCallResult<BittrexCurrency[]>> GetCurrenciesAsync()
        {
            return await Execute<BittrexCurrency[]>(GetUrl(CurrenciesEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        public WebCallResult<BittrexPrice> GetTicker(string market) => GetTickerAsync(market).Result;

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        public async Task<WebCallResult<BittrexPrice>> GetTickerAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            return await Execute<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        public WebCallResult<BittrexMarketSummary> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        public async Task<WebCallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            var result = await Execute<BittrexMarketSummary[]>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
            return new WebCallResult<BittrexMarketSummary>(result.ResponseStatusCode, result.ResponseHeaders, result.Data?.Any() == true ? result.Data[0]: null, result.Error);
        }

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        public WebCallResult<BittrexMarketSummary[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        public async Task<WebCallResult<BittrexMarketSummary[]>> GetMarketSummariesAsync()
        {
            return await Execute<BittrexMarketSummary[]>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public WebCallResult<BittrexOrderBook> GetOrderBook(string market) => GetOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public async Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "both" }
            };

            return await Execute<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public WebCallResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market) => GetBuyOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public async Task<WebCallResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "buy" }
            };

            return await Execute<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public WebCallResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market) => GetSellOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Order book for the market</returns>
        public async Task<WebCallResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "sell" }
            };

            return await Execute<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        public WebCallResult<BittrexMarketHistory[]> GetMarketHistory(string market) => GetMarketHistoryAsync(market).Result;

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        public async Task<WebCallResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            return await Execute<BittrexMarketHistory[]>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public WebCallResult<BittrexCandle[]> GetCandles(string market, TickInterval interval) => GetCandlesAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<WebCallResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval)
        {
            var parameters = new Dictionary<string, object>
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<BittrexCandle[]>(GetUrl(CandleEndpoint, Api, ApiVersion2), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public WebCallResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval) => GetLatestCandleAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<WebCallResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval)
        {
            var parameters = new Dictionary<string, object>
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<BittrexCandle[]>(GetUrl(LatestCandleEndpoint, Api, ApiVersion2), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        public WebCallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate) => PlaceOrderAsync(side, market, quantity, rate).Result;
        
        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        public async Task<WebCallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) }
            };

            var uri = GetUrl(side == OrderSide.Buy ? BuyLimitEndpoint : SellLimitEndpoint, Api, ApiVersion);
            return await Execute<BittrexGuid>(uri, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        public WebCallResult<object> CancelOrder(Guid guid) => CancelOrderAsync(guid).Result;
        
        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        public async Task<WebCallResult<object>> CancelOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };

            return await Execute<object>(GetUrl(CancelEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<BittrexOpenOrdersOrder[]> GetOpenOrders(string market = null) => GetOpenOrdersAsync(market).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<BittrexOpenOrdersOrder[]>> GetOpenOrdersAsync(string market = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", market);

            return await Execute<BittrexOpenOrdersOrder[]>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        public WebCallResult<BittrexBalance> GetBalance(string currency) => GetBalanceAsync(currency).Result;

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        public async Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        public WebCallResult<BittrexBalance[]> GetBalances() => GetBalancesAsync().Result;

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        public async Task<WebCallResult<BittrexBalance[]>> GetBalancesAsync()
        {
            return await Execute<BittrexBalance[]>(GetUrl(BalancesEndpoint, Api, ApiVersion), true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        public WebCallResult<BittrexDepositAddress> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        public async Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        public WebCallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null) => WithdrawAsync(currency, quantity, address, paymentId).Result;

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        public async Task<WebCallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null)
        {
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"address", address}
            };
            parameters.AddOptionalParameter("paymentid", paymentId);

            return await Execute<BittrexGuid>(GetUrl(WithdrawEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        public WebCallResult<BittrexAccountOrder> GetOrder(Guid guid) => GetOrderAsync(guid).Result;

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        public async Task<WebCallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };
            return await Execute<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        public WebCallResult<BittrexOrderHistoryOrder[]> GetOrderHistory(string market = null) => GetOrderHistoryAsync(market).Result;

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        public async Task<WebCallResult<BittrexOrderHistoryOrder[]>> GetOrderHistoryAsync(string market = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", market);
            return await Execute<BittrexOrderHistoryOrder[]>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        public WebCallResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null) => GetWithdrawalHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        public async Task<WebCallResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<BittrexWithdrawal[]>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<BittrexDeposit[]> GetDepositHistory(string currency = null) => GetDepositHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<BittrexDeposit[]>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }
        #endregion
        #region private
        /// <summary>
        /// Get url for endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="api"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected Uri GetUrl(string endpoint, string api, string version)
        {
            var address = BaseAddress;
            if (version == ApiVersion2)
                address = baseAddressV2;

            var result = $"{address}/{api}/v{version}/{endpoint}";
            return new Uri(result);
        }

        /// <inheritdoc />
        protected override bool IsErrorResponse(JToken data)
        {
            return data["success"] != null && !(bool) data["success"];
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken data)
        {
            if(data["message"] == null)
                return new UnknownError("Unknown response from server: " + data);

            return new ServerError((string)data["message"]);
        }

        private async Task<WebCallResult<T>> Execute<T>(Uri uri, bool signed = false, Dictionary<string, object> parameters = null, string method = "GET") where T: class
        {
            return GetResult(await ExecuteRequest<BittrexApiResult<T>>(uri, method, parameters, signed).ConfigureAwait(false));
        }
        
        private static WebCallResult<T> GetResult<T>(WebCallResult<BittrexApiResult<T>> result) where T : class
        {
            if (result.Error != null || result.Data == null)
                return WebCallResult<T>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);

            var messageEmpty = string.IsNullOrEmpty(result.Data.Message);
            return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, !messageEmpty ? null: result.Data.Result, !messageEmpty ? new ServerError(result.Data.Message): null);
        }
        
        private void Configure(BittrexClientOptions options)
        {
            base.Configure(options);
            if (options.ApiCredentials != null)
                SetAuthenticationProvider(new BittrexAuthenticationProvider(options.ApiCredentials));

            baseAddressV2 = options.BaseAddressV2;
        }

        #endregion
        #endregion
    }
}
