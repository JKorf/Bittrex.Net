using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Errors;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;
using Bittrex.Net.RateLimiter;
using Newtonsoft.Json;
using Bittrex.Net.Requests;
using Bittrex.Net.Converters;

namespace Bittrex.Net
{
    public class BittrexClient: BittrexAbstractClient, IBittrexClient
    {
        #region fields
        private string BaseAddress = "https://www.bittrex.com";
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

        private const string BalanceEndpoint = "account/getbalance";
        private const string BalancesEndpoint = "account/getbalances";
        private const string DepositAddressEndpoint = "account/getdepositaddress";
        private const string WithdrawEndpoint = "account/withdraw";
        private const string OrderEndpoint = "account/getorder";
        private const string OrderHistoryEndpoint = "account/getorderhistory";
        private const string WithdrawalHistoryEndpoint = "account/getwithdrawalhistory";
        private const string DepositHistoryEndpoint = "account/getdeposithistory";

        private List<IRateLimiter> rateLimiters = new List<IRateLimiter>();
        #endregion

        #region properties
        private long nonce => DateTime.UtcNow.Ticks;
        public IRequestFactory RequestFactory { get; set; } = new RequestFactory();

        /// <summary>
        /// The max amount of retries to do if the Bittrex service is temporarily unavailable
        /// </summary>
        public int MaxRetries { get; set; } = 2;
        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of BittrexClient
        /// </summary>
        public BittrexClient(): this(null)
        {   
        }

        /// <summary>
        /// Create a new instance of the BittrexClient
        /// </summary>
        /// <param name="baseUrl">Can be used to make calls to a different endpoint, for instance to use a mock server</param>
        public BittrexClient(string baseUrl)
        {
            if (baseUrl != null)
                BaseAddress = baseUrl;

            if (BittrexDefaults.MaxCallRetry != null)
                MaxRetries = BittrexDefaults.MaxCallRetry.Value;

            foreach (var rateLimiter in BittrexDefaults.RateLimiters)
                rateLimiters.Add(rateLimiter);
        }

        /// <summary>
        /// Create a new instance of BittrexClient using provided credentials. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret associated with the key</param>
        public BittrexClient(string apiKey, string apiSecret): this(apiKey, apiSecret, null)
        {
        }

        /// <summary>
        /// Create a new instance of BittrexClient using provided credentials. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret associated with the key</param>
        /// <param name="baseUrl">Can be used to make calls to a different endpoint, for instance to use a mock server</param>
        public BittrexClient(string apiKey, string apiSecret, string baseUrl)
        {
            if (baseUrl != null)
                BaseAddress = baseUrl;

            if (BittrexDefaults.MaxCallRetry != null)
                MaxRetries = BittrexDefaults.MaxCallRetry.Value;

            foreach (var rateLimiter in BittrexDefaults.RateLimiters)
                rateLimiters.Add(rateLimiter);

            SetApiCredentials(apiKey, apiSecret);
        }
        #endregion

        #region methods
        #region public

        /// <summary>
        /// Adds a rate limiter to the client. There are 2 choices, the <see cref="RateLimiterTotal"/> and the <see cref="RateLimiterPerEndpoint"/>.
        /// </summary>
        /// <param name="limiter">The limiter to add</param>
        public void AddRateLimiter(IRateLimiter limiter)
        {
            rateLimiters.Add(limiter);
        }

        /// <summary>
        /// Removes all rate limiters from this client
        /// </summary>
        public void RemoveRateLimiters()
        {
            rateLimiters.Clear();
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketsAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexMarket[]> GetMarkets() => GetMarketsAsync().Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <returns>List of markets</returns>
        public async Task<BittrexApiResult<BittrexMarket[]>> GetMarketsAsync()
        {
            return await ExecuteRequest<BittrexMarket[]>(GetUrl(MarketsEndpoint, Api, ApiVersion));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetCurrenciesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexCurrency[]> GetCurrencies() => GetCurrenciesAsync().Result;

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <returns>List of currencies</returns>
        public async Task<BittrexApiResult<BittrexCurrency[]>> GetCurrenciesAsync()
        {
            return await ExecuteRequest<BittrexCurrency[]>(GetUrl(CurrenciesEndpoint, Api, ApiVersion));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetTickerAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexPrice> GetTicker(string market) => GetTickerAsync(market).Result;

        /// <summary>
        /// Gets the price of a market
        /// </summary>
        /// <param name="market">Market to get price for</param>
        /// <returns>The current ask, bid and last prices for the market</returns>
        public async Task<BittrexApiResult<BittrexPrice>> GetTickerAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecuteRequest<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketSummaryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexMarketSummary> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;

        /// <summary>
        /// Gets a summary of the market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <returns>List with single entry containing info for the market</returns>
        public async Task<BittrexApiResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            var result = await ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion, parameters));
            if (!result.Success || result.Result.Length == 0)
                return ThrowErrorMessage<BittrexMarketSummary>(result.Error);

            return new BittrexApiResult<BittrexMarketSummary>() { Result = result.Result[0], Success = true, Error = result.Error};
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketSummariesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexMarketSummary[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;

        /// <summary>
        /// Gets a summary for all markets
        /// </summary>
        /// <returns>List of summaries for all markets</returns>
        public async Task<BittrexApiResult<BittrexMarketSummary[]>> GetMarketSummariesAsync()
        {
            return await ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexOrderBook> GetOrderBook(string market) => GetOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids and asks for a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<BittrexApiResult<BittrexOrderBook>> GetOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "both" }
            };

            return await ExecuteRequest<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBuyOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market) => GetBuyOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with asks for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "buy" }
            };

            return await ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetSellOrderBookAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market) => GetSellOrderBookAsync(market).Result;

        /// <summary>
        /// Gets the order book with bids for a market
        /// </summary>
        /// <param name="market">Market to get the order book for</param>
        /// <returns>Orderbook for the market</returns>
        public async Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "sell" }
            };

            return await ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetMarketHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexMarketHistory[]> GetMarketHistory(string market) => GetMarketHistoryAsync(market).Result;

        /// <summary>
        /// Gets the last trades on a market
        /// </summary>
        /// <param name="market">Market to get history for</param>
        /// <returns>List of trade aggregations</returns>
        public async Task<BittrexApiResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecuteRequest<BittrexMarketHistory[]>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion, parameters));
        }
        
        /// <summary>
        /// Synchronized version of the <see cref="GetCandlesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexCandle[]> GetCandles(string market, TickInterval interval) => GetCandlesAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="market">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<BittrexApiResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await ExecuteRequest<BittrexCandle[]>(GetUrl(CandleEndpoint, Api, ApiVersion2, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetLatestCandleAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval) => GetLatestCandleAsync(market, interval).Result;

        /// <summary>
        /// Gets candle data for a market on a specific interval
        /// </summary>
        /// <param name="market">Market to get candles for</param>
        /// <param name="market">The candle interval</param>
        /// <returns>List of candles</returns>
        public async Task<BittrexApiResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "marketName", market },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await ExecuteRequest<BittrexCandle[]>(GetUrl(LatestCandleEndpoint, Api, ApiVersion2, parameters));
        }

        /// <summary>
        /// Synchronized version of the <see cref="PlaceOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexGuid> PlaceOrder(OrderType type, string market, decimal quantity, decimal rate) => PlaceOrderAsync(type, market, quantity, rate).Result;
        
        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="type">Type of the order</param>
        /// <param name="market">Market to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <returns></returns>
        public async Task<BittrexApiResult<BittrexGuid>> PlaceOrderAsync(OrderType type, string market, decimal quantity, decimal rate)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexGuid>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));
            
            var parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) }
            };

            var uri = GetUrl(type == OrderType.Buy ? BuyLimitEndpoint : SellLimitEndpoint, Api, ApiVersion, parameters);
            return await ExecuteRequest<BittrexGuid>(uri, true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="CancelOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<object> CancelOrder(Guid guid) => CancelOrderAsync(guid).Result;
        
        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <returns></returns>
        public async Task<BittrexApiResult<object>> CancelOrderAsync(Guid guid)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<object>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };

            return await ExecuteRequest<object>(GetUrl(CancelEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOpenOrdersAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexOrder[]> GetOpenOrders(string market = null) => GetOpenOrdersAsync(market).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="market">Filter list by market</param>
        /// <returns>List of open orders</returns>
        public async Task<BittrexApiResult<BittrexOrder[]>> GetOpenOrdersAsync(string market = null)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexOrder[]>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);

            return await ExecuteRequest<BittrexOrder[]>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBalanceAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexBalance> GetBalance(string currency) => GetBalanceAsync(currency).Result;

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <returns>The balance of the currency</returns>
        public async Task<BittrexApiResult<BittrexBalance>> GetBalanceAsync(string currency)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexBalance>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return await ExecuteRequest<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetBalancesAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexBalance[]> GetBalances() => GetBalancesAsync().Result;

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <returns>List of balances</returns>
        public async Task<BittrexApiResult<BittrexBalance[]>> GetBalancesAsync()
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexBalance[]>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            return await ExecuteRequest<BittrexBalance[]>(GetUrl(BalancesEndpoint, Api, ApiVersion), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetDepositAddressAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexDepositAddress> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;

        /// <summary>
        /// Gets the desposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <returns>The deposit address of the currency</returns>
        public async Task<BittrexApiResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexDepositAddress>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return await ExecuteRequest<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="WithdrawAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null) => WithdrawAsync(currency, quantity, address, paymentId).Result;

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <returns>Guid of the withdrawal</returns>
        public async Task<BittrexApiResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexGuid>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"address", address},
            };
            AddOptionalParameter(parameters, "paymentid", paymentId);

            return await ExecuteRequest<BittrexGuid>(GetUrl(WithdrawEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOrderAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexAccountOrder> GetOrder(Guid guid) => GetOrderAsync(guid).Result;

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <returns>The requested order</returns>
        public async Task<BittrexApiResult<BittrexAccountOrder>> GetOrderAsync(Guid guid)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexAccountOrder>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };
            return await ExecuteRequest<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetOrderHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexOrder[]> GetOrderHistory(string market = null) => GetOrderHistoryAsync(market).Result;

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="market">Filter on market</param>
        /// <returns>List of orders</returns>
        public async Task<BittrexApiResult<BittrexOrder[]>> GetOrderHistoryAsync(string market = null)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexOrder[]>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);
            return await ExecuteRequest<BittrexOrder[]>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetWithdrawalHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null) => GetWithdrawalHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of withdrawals</returns>
        public async Task<BittrexApiResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexWithdrawal[]>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return await ExecuteRequest<BittrexWithdrawal[]>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion, parameters), true);
        }

        /// <summary>
        /// Synchronized version of the <see cref="GetDepositHistoryAsync"/> method
        /// </summary>
        /// <returns></returns>
        public BittrexApiResult<BittrexDeposit[]> GetDepositHistory(string currency = null) => GetDepositHistoryAsync(currency).Result;

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <returns>List of deposits</returns>
        public async Task<BittrexApiResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null)
        {
            if (apiKey == null || encryptor == null)
                return ThrowErrorMessage<BittrexDeposit[]>(BittrexErrors.GetError(BittrexErrorKey.NoApiCredentialsProvided));

            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return await ExecuteRequest<BittrexDeposit[]>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion, parameters), true);
        }
        #endregion
        #region private
        protected async Task<BittrexApiResult<T>> ExecuteRequest<T>(Uri uri, bool signed = false, int currentTry = 0)
        {
            string returnedData = "";
            try
            {
                var uriString = uri.ToString();
                if (signed)
                {
                    if (!uriString.EndsWith("?"))
                        uriString += "&";

                    uriString += $"apiKey={apiKey}&nonce={nonce}";
                }

                var request = RequestFactory.Create(uriString);
                if (signed)
                    request.Headers.Add("apisign",
                        ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(uriString))));

                foreach (var limiter in rateLimiters)
                {
                    double limitedBy = limiter.LimitRequest(uri.AbsolutePath);
                    if(limitedBy > 0)
                        log.Write(LogVerbosity.Debug, $"Request {uri.AbsolutePath} was limited by {limitedBy}ms by {limiter.GetType().Name}");
                }

                log.Write(LogVerbosity.Debug, $"Sending request to {uriString}");
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    returnedData = await reader.ReadToEndAsync();
                    var result = JsonConvert.DeserializeObject<BittrexApiResult<T>>(returnedData);
                    if (!result.Success)
                    {
                        log.Write(LogVerbosity.Debug, $"Call failed to {uri}, Error message: {result.Message}");
                        result.Error = new BittrexError(6000, result.Message);
                        result.Message = null;
                    }
                    return result;
                }
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse) we.Response;
                if (currentTry < MaxRetries)
                    return await ExecuteRequest<T>(uri, signed, ++currentTry);

                return ThrowErrorMessage<T>(BittrexErrors.GetError(BittrexErrorKey.ErrorWeb), $"Status: {response.StatusCode}-{response.StatusDescription}, Message: {we.Message}");
            }
            catch (JsonReaderException jre)
            {
                return ThrowErrorMessage<T>(BittrexErrors.GetError(BittrexErrorKey.ParseErrorReader), $"Error occured at Path: {jre.Path}, LineNumber: {jre.LineNumber}, LinePosition: {jre.LinePosition}. Received data: {returnedData}");
            }
            catch (JsonSerializationException jse)
            {
                return ThrowErrorMessage<T>(BittrexErrors.GetError(BittrexErrorKey.ParseErrorSerialization), $"Message: {jse.Message}. Received data: {returnedData}");
            }
            catch (Exception e)
            {
                return ThrowErrorMessage<T>(BittrexErrors.GetError(BittrexErrorKey.UnknownError), $"Message: {e.Message}");
            }
        }

        protected Uri GetUrl(string endpoint, string api, string version, Dictionary<string, string> parameters = null)
        {
            var result = $"{BaseAddress}/{api}/v{version}/{endpoint}?";
            if (parameters != null)
                result += $"{string.Join("&", parameters.Select(s => $"{s.Key}={s.Value}"))}";
            return new Uri(result);
        }

        private void AddOptionalParameter(Dictionary<string, string> dictionary, string key, string value)
        {
            if (value != null)
                dictionary.Add(key, value);
        }

        private string ByteToString(byte[] buff)
        {
            var sbinary = "";
            foreach (byte t in buff)
                sbinary += t.ToString("X2"); /* hex format */
            return sbinary;
        }
        #endregion
        #endregion
    }
}
