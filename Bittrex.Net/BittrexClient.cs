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

namespace Bittrex.Net
{
    public class BittrexClient: ExchangeClient, IBittrexClient
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();
        
        private const string Api = "api";
        private const string ApiVersion = "1.1";
        private const string ApiVersion2 = "2.0";

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
        private const string SellV2Endpoint = "key/market/tradesell";
        private const string BuyV2Endpoint = "key/market/tradebuy";

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
        public BittrexClient(): this(defaultOptions)
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
        /// Synchronized version of the <see cref="GetMarketsAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexMarket[]> GetMarkets() => GetMarketsAsync().Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public async Task<CallResult<BittrexMarket[]>> GetMarketsAsync()
        {
            return await Execute<BittrexMarket[]>(GetUrl(MarketsEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetCurrenciesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexCurrency[]> GetCurrencies() => GetCurrenciesAsync().Result;

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public async Task<CallResult<BittrexCurrency[]>> GetCurrenciesAsync()
        {
            return await Execute<BittrexCurrency[]>(GetUrl(CurrenciesEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetTickerAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexPrice> GetTicker(string market) => GetTickerAsync(market).Result;

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        public async Task<CallResult<BittrexPrice>> GetTickerAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            return await Execute<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketSummaryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexMarketSummary> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        public async Task<CallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            var result = await Execute<BittrexMarketSummary[]>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
            return new CallResult<BittrexMarketSummary>(result.Data.Any() ? result.Data[0]: null, result.Error);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketSummariesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexMarketSummary[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        public async Task<CallResult<BittrexMarketSummary[]>> GetMarketSummariesAsync()
        {
            return await Execute<BittrexMarketSummary[]>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion)).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOrderBook> GetOrderBook(string market) => GetOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<CallResult<BittrexOrderBook>> GetOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "both" }
            };

            return await Execute<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBuyOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market) => GetBuyOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<CallResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "buy" }
            };

            return await Execute<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetSellOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market) => GetSellOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<CallResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market },
                { "type", "sell" }
            };

            return await Execute<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexMarketHistory[]> GetMarketHistory(string market) => GetMarketHistoryAsync(market).Result;

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        public async Task<CallResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", market }
            };

            return await Execute<BittrexMarketHistory[]>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion), false, parameters).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Synchronized version of the <see cref="GetCandlesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexCandle[]> GetCandles(string market, TickInterval interval) => GetCandlesAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<CallResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval)
        {
            var parameters = new Dictionary<string, object>
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<BittrexCandle[]>(GetUrl(CandleEndpoint, Api, ApiVersion2), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetLatestCandleAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval) => GetLatestCandleAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="interval">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<CallResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval)
        {
            var parameters = new Dictionary<string, object>
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<BittrexCandle[]>(GetUrl(LatestCandleEndpoint, Api, ApiVersion2), false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="PlaceOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate) => PlaceOrderAsync(side, market, quantity, rate).Result;
        
        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        public async Task<CallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate)
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
        /// Synchronized version of the <see cref="PlaceConditionalOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOrderResult> PlaceConditionalOrder(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target) => PlaceConditionalOrderAsync(side, timeInEffect, market, quantity, rate, conditionType, target).Result;

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
        public async Task<CallResult<BittrexOrderResult>> PlaceConditionalOrderAsync(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ordertype", OrderType.Limit.ToString() },
                { "timeineffect", JsonConvert.SerializeObject(timeInEffect, new TimeInEffectConverter(false)) },
                { "marketname", market },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) },
                { "conditiontype", JsonConvert.SerializeObject(conditionType, new ConditionTypeConverter(false)) },
                { "target", target.ToString(CultureInfo.InvariantCulture) }
            };

            var uri = GetUrl(side == OrderSide.Buy ? BuyV2Endpoint : SellV2Endpoint, Api, ApiVersion2);
            return await Execute<BittrexOrderResult>(uri, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="CancelOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<object> CancelOrder(Guid guid) => CancelOrderAsync(guid).Result;
        
        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        public async Task<CallResult<object>> CancelOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };

            return await Execute<object>(GetUrl(CancelEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOpenOrdersAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOpenOrdersOrder[]> GetOpenOrders(string market = null) => GetOpenOrdersAsync(market).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        public async Task<CallResult<BittrexOpenOrdersOrder[]>> GetOpenOrdersAsync(string market = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", market);

            return await Execute<BittrexOpenOrdersOrder[]>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBalanceAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexBalance> GetBalance(string currency) => GetBalanceAsync(currency).Result;

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        public async Task<CallResult<BittrexBalance>> GetBalanceAsync(string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBalancesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexBalance[]> GetBalances() => GetBalancesAsync().Result;

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        public async Task<CallResult<BittrexBalance[]>> GetBalancesAsync()
        {
            return await Execute<BittrexBalance[]>(GetUrl(BalancesEndpoint, Api, ApiVersion), true).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetDepositAddressAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexDepositAddress> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;

        /// <summary>
        /// Gets the desposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        public async Task<CallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="WithdrawAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null) => WithdrawAsync(currency, quantity, address, paymentId).Result;

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        public async Task<CallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null)
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
        /// Synchronized version of the <see cref="GetOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexAccountOrder> GetOrder(Guid guid) => GetOrderAsync(guid).Result;

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        public async Task<CallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };
            return await Execute<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOrderHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexOrderHistoryOrder[]> GetOrderHistory(string market = null) => GetOrderHistoryAsync(market).Result;

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        public async Task<CallResult<BittrexOrderHistoryOrder[]>> GetOrderHistoryAsync(string market = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", market);
            return await Execute<BittrexOrderHistoryOrder[]>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetWithdrawalHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null) => GetWithdrawalHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        public async Task<CallResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<BittrexWithdrawal[]>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetDepositHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public CallResult<BittrexDeposit[]> GetDepositHistory(string currency = null) => GetDepositHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        public async Task<CallResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<BittrexDeposit[]>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion), true, parameters).ConfigureAwait(false);
        }
        #endregion
        #region private

        protected Uri GetUrl(string endpoint, string api, string version)
        {
            var result = $"{baseAddress}/{api}/v{version}/{endpoint}";
            return new Uri(result);
        }

        private async Task<CallResult<T>> Execute<T>(Uri uri, bool signed = false, Dictionary<string, object> parameters = null) where T: class
        {
            return GetResult(await ExecuteRequest<BittrexApiResult<T>>(uri, "GET", parameters, signed).ConfigureAwait(false));
        }

        private static CallResult<T> GetResult<T>(CallResult<BittrexApiResult<T>> result) where T : class
        {
            if (result.Error != null || result.Data == null)
                return new CallResult<T>(null, result.Error);

            var messageEmpty = string.IsNullOrEmpty(result.Data.Message);
            return new CallResult<T>(!messageEmpty ? null: result.Data.Result, !messageEmpty ? new ServerError(result.Data.Message): null);
        }
        
        private void Configure(BittrexClientOptions options)
        {
            base.Configure(options);
            if (options.ApiCredentials != null)
                SetAuthenticationProvider(new BittrexAuthenticationProvider(options.ApiCredentials));

            baseAddress = options.BaseAddress;
        }

        #endregion
        #endregion
    }
}
