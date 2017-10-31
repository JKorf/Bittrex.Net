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
    public class BittrexClient: BittrexAbstractClient
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
        #endregion

        #region methods
        #region public
        public BittrexApiResult<BittrexMarket[]> GetMarkets()
        {
            return ExecuteRequest<BittrexMarket[]>(GetUrl(MarketsEndpoint, Api, ApiVersion)).Result;
        }

        public BittrexApiResult<BittrexCurrencies[]> GetCurrencies()
        {
            return ExecuteRequest<BittrexCurrencies[]>(GetUrl(CurrenciesEndpoint, Api, ApiVersion)).Result;
        }

        public BittrexApiResult<BittrexPrice> GetTicker(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return ExecuteRequest<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexMarketSummary[]> GetMarketSummary(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexMarketSummary[]> GetMarketSummaries()
        {
            return ExecuteRequest<BittrexMarketSummary[]>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion)).Result;
        }

        public BittrexApiResult<BittrexOrderBook> GetOrderBook(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "both" }
            };

            return ExecuteRequest<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "buy" }
            };

            return ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market },
                { "type", "sell" }
            };

            return ExecuteRequest<BittrexOrderBookEntry[]>(GetUrl(OrderBookEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexMarketHistory[]> GetMarketHistory(string market)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "market", market }
            };

            return ExecuteRequest<BittrexMarketHistory[]>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion, parameters)).Result;
        }

        public BittrexApiResult<BittrexGuid> PlaceOrder(OrderType type, string market, double quantity, double rate)
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

            return ExecuteRequest<BittrexGuid>(uri, true).Result;
        }

        public BittrexApiResult<object> CancelOrder(Guid guid)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };

            return ExecuteRequest<object>(GetUrl(CancelEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexOrder[]> GetOpenOrders(string market = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);

            return ExecuteRequest<BittrexOrder[]>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexBalance> GetBalance(string currency)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return ExecuteRequest<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexBalance[]> GetBalances()
        {
            return ExecuteRequest<BittrexBalance[]>(GetUrl(BalancesEndpoint, Api, ApiVersion), true).Result;
        }

        public BittrexApiResult<BittrexDepositAddress> GetDepositAddress(string currency)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency}
            };
            return ExecuteRequest<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexGuid> Withdraw(string currency, double quantity, string address, string paymentId = null)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"currency", currency},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"address", address},
            };
            AddOptionalParameter(parameters, "paymentid", paymentId);

            return ExecuteRequest<BittrexGuid>(GetUrl(WithdrawEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexAccountOrder> GetOrder(Guid guid)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"uuid", guid.ToString()}
            };
            return ExecuteRequest<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexOrder[]> GetOrderHistory(string market = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "market", market);
            return ExecuteRequest<BittrexOrder[]>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return ExecuteRequest<BittrexWithdrawal[]>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion, parameters), true).Result;
        }

        public BittrexApiResult<BittrexDeposit[]> GetDepositHistory(string currency = null)
        {
            var parameters = new Dictionary<string, string>();
            AddOptionalParameter(parameters, "currency", currency);
            return ExecuteRequest<BittrexDeposit[]>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion, parameters), true).Result;
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
        #endregion
        #endregion
    }
}
