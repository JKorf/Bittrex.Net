using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Implementations;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net
{
    public class BittrexClient: BittrexAbstractClient, IDisposable
    {
        #region fields
        private const string BaseAddress = "https://www.bittrex.com";
        private const string Api = "api";
        private const string ApiVersion = "1.1";

        private const string MarketsEndpoint = "public/getmarkets";
        private const string CurrenciesEndpoint = "public/getcurrencies";
        private const string TickerEndpoint = "public/getticker";
        private const string MarketSummariesEndpoint = "public/getmarketsummaries";
        private const string MarketSummaryEndpoint = "public/getmarketsummary";
        private const string OrderBookEndpoint = "public/getorderbook";
        private const string MarketHistoryEndpoint = "public/getmarkethistory";

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
        #endregion

        #region properties
        private long nonce => DateTime.UtcNow.Ticks;
        public IRequestFactory RequestFactory { get; set; } = new RequestFactory();
        #endregion

        #region constructor/destructor
        public BittrexClient()
        {
        }

        public BittrexClient(string apiKey, string apiSecret)
        {
            SetApiCredentials(apiKey, apiSecret);
        }

        ~BittrexClient()
        {
            Dispose(false);
        }
        #endregion

        #region methods
        #region public

        public BittrexApiResult<BittrexMarket[]> GetMarkets() => GetMarketsAsync().Result;
        public async Task<BittrexApiResult<BittrexMarket[]>> GetMarketsAsync()
        {
            return await ExecuteRequest<BittrexMarket[]>(GetUrl(MarketsEndpoint, Api, ApiVersion));
        }

        public BittrexApiResult<BittrexCurrencies[]> GetCurrencies() => GetCurrenciesAsync().Result;
        public async Task<BittrexApiResult<BittrexCurrencies[]>> GetCurrenciesAsync()
        {
            return await ExecuteRequest<BittrexCurrencies[]>(GetUrl(CurrenciesEndpoint, Api, ApiVersion));
        }

        public BittrexApiResult<BittrexPrice> GetTicker(string market) => GetTickerAsync(market).Result;
        public async Task<BittrexApiResult<BittrexPrice>> GetTickerAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecuteRequest<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexMarketSummary[]> GetMarketSummary(string market) => GetMarketSummaryAsync(market).Result;
        public async Task<BittrexApiResult<BittrexMarketSummary[]>> GetMarketSummaryAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexMarketSummary[]> GetMarketSummaries() => GetMarketSummariesAsync().Result;
        public async Task<BittrexApiResult<BittrexMarketSummary[]>> GetMarketSummariesAsync()
        {
            return await ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion));
        }

        public BittrexApiResult<BittrexOrderBook> GetOrderBook(string market) => GetOrderBookAsync(market).Result;
        public async Task<BittrexApiResult<BittrexOrderBook>> GetOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "both" }
            };

            return await ExecuteRequest<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market) => GetBuyOrderBookAsync(market).Result;
        public async Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "buy" }
            };

            return await ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market) => GetSellOrderBookAsync(market).Result;
        public async Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "sell" }
            };

            return await ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexMarketHistory[]> GetMarketHistory(string market) => GetMarketHistoryAsync(market).Result;
        public async Task<BittrexApiResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return await ExecuteRequest<BittrexMarketHistory[]>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion, parameters));
        }

        public BittrexApiResult<BittrexGuid> PlaceOrder(OrderType type, string market, double quantity, double rate) => PlaceOrderAsync(type, market, quantity, rate).Result;
        public async Task<BittrexApiResult<BittrexGuid>> PlaceOrderAsync(OrderType type, string market, double quantity, double rate)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) }
            };

            Uri uri;
            if (type == OrderType.Buy)
                uri = GetUrl(BuyLimitEndpoint, Api, ApiVersion, parameters);
            else
                uri = GetUrl(SellLimitEndpoint, Api, ApiVersion, parameters);

            return await ExecuteRequest<BittrexGuid>(uri, true);
        }

        public BittrexApiResult<object> CancelOrder(Guid guid) => CancelOrderAsync(guid).Result;
        public async Task<BittrexApiResult<object>> CancelOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };

            return await ExecuteRequest<object>(GetUrl(CancelEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexOrder[]> GetOpenOrders(string market = null) => GetOpenOrdersAsync(market).Result;
        public async Task<BittrexApiResult<BittrexOrder[]>> GetOpenOrdersAsync(string market = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);

            return await ExecuteRequest<BittrexOrder[]>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexBalance> GetBalance(string currency) => GetBalanceAsync(currency).Result;
        public async Task<BittrexApiResult<BittrexBalance>> GetBalanceAsync(string currency)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return await ExecuteRequest<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexBalance[]> GetBalances() => GetBalancesAsync().Result;
        public async Task<BittrexApiResult<BittrexBalance[]>> GetBalancesAsync()
        {
            return await ExecuteRequest<BittrexBalance[]>(GetUrl(BalancesEndpoint, Api, ApiVersion), true);
        }

        public BittrexApiResult<BittrexDepositAddress> GetDepositAddress(string currency) => GetDepositAddressAsync(currency).Result;
        public async Task<BittrexApiResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return await ExecuteRequest<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexGuid> Withdraw(string currency, double quantity, string address, string paymentId = null) => WithdrawAsync(currency, quantity, address, paymentId).Result;
        public async Task<BittrexApiResult<BittrexGuid>> WithdrawAsync(string currency, double quantity, string address, string paymentId = null)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"address", address},
            };
            AddOptionalParameter(parameters, "paymentid", paymentId);

            return await ExecuteRequest<BittrexGuid>(GetUrl(WithdrawEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexAccountOrder> GetOrder(Guid guid) => GetOrderAsync(guid).Result;
        public async Task<BittrexApiResult<BittrexAccountOrder>> GetOrderAsync(Guid guid)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };
            return await ExecuteRequest<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexOrder[]> GetOrderHistory(string market = null) => GetOrderHistoryAsync(market).Result;
        public async Task<BittrexApiResult<BittrexOrder[]>> GetOrderHistoryAsync(string market = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);
            return await ExecuteRequest<BittrexOrder[]>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null) => GetWithdrawalHistoryAsync(currency).Result;
        public async Task<BittrexApiResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return await ExecuteRequest<BittrexWithdrawal[]>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion, parameters), true);
        }

        public BittrexApiResult<BittrexDeposit[]> GetDepositHistory(string currency = null) => GetDepositHistoryAsync(currency).Result;
        public async Task<BittrexApiResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return await ExecuteRequest<BittrexDeposit[]>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion, parameters), true);
        }
        #endregion
        #region private
        private async Task<BittrexApiResult<T>> ExecuteRequest<T>(Uri uri, bool signed = false)
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
                    request.Headers.Add("apisign", ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(uriString))));

                log.Write(LogVerbosity.Debug, $"Sending request to {uriString}");
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    returnedData = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<BittrexApiResult<T>>(returnedData);
                }
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                var errorMessage = $"Request to {uri} failed because of a webexception. Status: {response.StatusCode}-{response.StatusDescription}, Message: {we.Message}";
                log.Write(LogVerbosity.Warning, errorMessage);
                return ThrowErrorMessage<T>(errorMessage);
            }
            catch (JsonReaderException jre)
            {
                var errorMessage = $"Request to {uri} failed, couldn't parse the returned data. Error occured at Path: {jre.Path}, LineNumber: {jre.LineNumber}, LinePosition: {jre.LinePosition}. Received data: {returnedData}";
                log.Write(LogVerbosity.Warning, errorMessage);
                return ThrowErrorMessage<T>(errorMessage);
            }
            catch (Exception e)
            {
                var errorMessage = $"Request to {uri} failed with unknown error: " + e.Message;
                log.Write(LogVerbosity.Warning, errorMessage);
                return ThrowErrorMessage<T>(errorMessage);
            }
        }

        private Uri GetUrl(string endpoint, string api, string version, Dictionary<string, string> parameters = null)
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

        private void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        #endregion
    }
}
