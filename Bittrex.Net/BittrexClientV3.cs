using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
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
    /// Client for the Bittrex V3 API
    /// </summary>
    public class BittrexClientV3 : RestClient, IBittrexClientV3
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>Time of the server</returns>
        public WebCallResult<DateTime> GetServerTime(CancellationToken ct = default) => GetServerTimeAsync(ct).Result;

        /// <summary>
        /// Gets the server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Time of the server</returns>
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BittrexServerTimeV3>(GetUrl("ping"), HttpMethod.Get, ct).ConfigureAwait(false);
            return new WebCallResult<DateTime>(result.ResponseStatusCode, result.ResponseHeaders, result.Data?.ServerTime ?? default, result.Error);
        }

        #region symbols
        /// <summary>
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        public WebCallResult<IEnumerable<BittrexSymbolV3>> GetSymbols(CancellationToken ct = default) => GetSymbolsAsync(ct).Result;

        /// <summary>
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        public async Task<WebCallResult<IEnumerable<BittrexSymbolV3>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexSymbolV3>>(GetUrl("markets"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets information about a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        public WebCallResult<BittrexSymbolV3> GetSymbol(string symbol, CancellationToken ct = default) => GetSymbolAsync(symbol, ct).Result;

        /// <summary>
        /// Gets information about a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol info</returns>
        public async Task<WebCallResult<BittrexSymbolV3>> GetSymbolAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequest<BittrexSymbolV3>(GetUrl("markets/" + symbol), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summaries of all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbol summaries</returns>
        public WebCallResult<IEnumerable<BittrexSymbolSummaryV3>> GetSymbolSummaries(CancellationToken ct = default) => GetSymbolSummariesAsync(ct).Result;

        /// <summary>
        /// Gets summaries of all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbol summaries</returns>
        public async Task<WebCallResult<IEnumerable<BittrexSymbolSummaryV3>>> GetSymbolSummariesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexSymbolSummaryV3>>(GetUrl("markets/summaries"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summary of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol summary</returns>
        public WebCallResult<BittrexSymbolSummaryV3> GetSymbolSummary(string symbol, CancellationToken ct = default) => GetSymbolSummaryAsync(symbol, ct).Result;

        /// <summary>
        /// Gets summary of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol summary</returns>
        public async Task<WebCallResult<BittrexSymbolSummaryV3>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequest<BittrexSymbolSummaryV3>(GetUrl($"markets/{symbol}/summary"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The number of results per side for the order book (1, 25 or 500)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol order book</returns>
        public WebCallResult<BittrexOrderBookV3> GetOrderBook(string symbol, int? limit = null, CancellationToken ct = default) => GetOrderBookAsync(symbol, limit, ct).Result;

        /// <summary>
        /// Gets the order book of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The number of results per side for the order book (1, 25 or 500)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol order book</returns>
        public async Task<WebCallResult<BittrexOrderBookV3>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            limit?.ValidateIntValues(nameof(limit), 1, 25, 500);
            
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("depth", limit?.ToString(CultureInfo.InvariantCulture));

            var result = await SendRequest<BittrexOrderBookV3>(GetUrl($"markets/{symbol}/orderbook"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if(result.Data != null)
                result.Data.Sequence = result.ResponseHeaders!.GetSequence() ?? 0;
            return result;
        }

        /// <summary>
        /// Gets the trade history of a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol trade list</returns>
        public WebCallResult<IEnumerable<Objects.V3.BittrexSymbolTrade>> GetSymbolTrades(string symbol, CancellationToken ct = default) => GetSymbolTradesAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the trade history of a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol trade list</returns>
        public async Task<WebCallResult<IEnumerable<Objects.V3.BittrexSymbolTrade>>> GetSymbolTradesAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequest<IEnumerable<Objects.V3.BittrexSymbolTrade>>(GetUrl($"markets/{symbol}/trades"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticker of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol ticker</returns>
        public WebCallResult<BittrexTickV3> GetTicker(string symbol, CancellationToken ct = default) => GetTickerAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the ticker of a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol ticker</returns>
        public async Task<WebCallResult<BittrexTickV3>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var result = await SendRequest<BittrexTickV3>(GetUrl($"markets/{symbol}/ticker"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (result.Success)
                result.Data.Symbol = symbol;
            return result;
        }

        /// <summary>
        /// Gets list of tickers for all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol tickers</returns>
        public WebCallResult<IEnumerable<BittrexTickV3>> GetTickers(CancellationToken ct = default) => GetTickersAsync(ct).Result;

        /// <summary>
        /// Gets list of tickers for all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol tickers</returns>
        public async Task<WebCallResult<IEnumerable<BittrexTickV3>>> GetTickersAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexTickV3>>(GetUrl("markets/tickers"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the klines for a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol kline</returns>
        public WebCallResult<IEnumerable<BittrexKlineV3>> GetKlines(string symbol, KlineInterval interval, CancellationToken ct = default) => GetKlinesAsync(symbol, interval, ct).Result;

        /// <summary>
        /// Gets the klines for a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol klines</returns>
        public async Task<WebCallResult<IEnumerable<BittrexKlineV3>>> GetKlinesAsync(string symbol, KlineInterval interval, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            return await SendRequest<IEnumerable<BittrexKlineV3>>(GetUrl($"markets/{symbol}/candles/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/recent"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

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
        public WebCallResult<IEnumerable<BittrexKlineV3>> GetHistoricalKlines(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, CancellationToken ct = default) => GetHistoricalKlinesAsync(symbol, interval, year, month, day, ct).Result;

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
        public async Task<WebCallResult<IEnumerable<BittrexKlineV3>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            if(interval == KlineInterval.OneDay && month.HasValue)
                throw new ArgumentException("Can't specify month value when using day interval");

            if (interval == KlineInterval.OneHour && day.HasValue)
                throw new ArgumentException("Can't specify day value when using hour interval");

            if (day.HasValue && !month.HasValue)
                throw new ArgumentException("Can't specify day value without month value");

            var url =
                $"markets/{symbol}/candles/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/historical/{year}";
            if (month.HasValue)
                url += "/" + month;
            if (day.HasValue)
                url += "/" + day;

            return await SendRequest<IEnumerable<BittrexKlineV3>>(GetUrl(url), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        #endregion

        #region currencies
        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        public WebCallResult<IEnumerable<BittrexCurrencyV3>> GetCurrencies(CancellationToken ct = default) => GetCurrenciesAsync(ct).Result;

        /// <summary>
        /// Gets a list of all currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        public async Task<WebCallResult<IEnumerable<BittrexCurrencyV3>>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexCurrencyV3>>(GetUrl("currencies"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Currency info</returns>
        public WebCallResult<BittrexCurrencyV3> GetCurrency(string currency, CancellationToken ct = default) => GetCurrencyAsync(currency, ct).Result;

        /// <summary>
        /// Gets info on a currency
        /// </summary>
        /// <param name="currency">The name of the currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Currency info</returns>
        public async Task<WebCallResult<BittrexCurrencyV3>> GetCurrencyAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            return await SendRequest<BittrexCurrencyV3>(GetUrl($"currencies/{currency}"), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        #endregion

        #region accounts
        /// <summary>
        /// Get account info
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account info</returns>
        public WebCallResult<BittrexAccount> GetAccount(CancellationToken ct = default) => GetAccountAsync(ct).Result;

        /// <summary>
        /// Get account info
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account info</returns>
        public async Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default)
        {
            return await SendRequest<BittrexAccount>(GetUrl("account"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get account volume
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account volume</returns>
        public WebCallResult<BittrexAccountVolume> GetAccountVolume(CancellationToken ct = default) => GetAccountVolumeAsync(ct).Result;

        /// <summary>
        /// Get account volume
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account volume</returns>
        public async Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default)
        {
            return await SendRequest<BittrexAccountVolume>(GetUrl("account/volume"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region balances

        /// <summary>
        /// Gets current balances. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        public WebCallResult<IEnumerable<BittrexBalanceV3>> GetBalances(CancellationToken ct = default) => GetBalancesAsync(ct).Result;

        /// <summary>
        /// Gets current balances. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        public async Task<WebCallResult<IEnumerable<BittrexBalanceV3>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexBalanceV3>>(GetUrl("balances"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for currency</returns>
        public WebCallResult<BittrexBalanceV3> GetBalance(string currency, CancellationToken ct = default) => GetBalanceAsync(currency, ct).Result;

        /// <summary>
        /// Gets current balance for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for currency</returns>
        public async Task<WebCallResult<BittrexBalanceV3>> GetBalanceAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            return await SendRequest<BittrexBalanceV3>(GetUrl($"balances/{currency}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region addresses
        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposit addresses</returns>
        public WebCallResult<IEnumerable<BittrexDepositAddressV3>> GetDepositAddresses(CancellationToken ct = default) => GetDepositAddressesAsync(ct).Result;

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposit addresses</returns>
        public async Task<WebCallResult<IEnumerable<BittrexDepositAddressV3>>> GetDepositAddressesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexDepositAddressV3>>(GetUrl("addresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit addresses</returns>
        public WebCallResult<BittrexDepositAddressV3> GetDepositAddress(string currency, CancellationToken ct = default) => GetDepositAddressAsync(currency, ct).Result;

        /// <summary>
        /// Gets deposit addresses for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get the deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit addresses</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3>> GetDepositAddressAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            return await SendRequest<BittrexDepositAddressV3>(GetUrl($"addresses/{currency}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address</returns>
        public WebCallResult<BittrexDepositAddressV3> RequestDepositAddress(string currency, CancellationToken ct = default) => RequestDepositAddressAsync(currency, ct).Result;

        /// <summary>
        /// Request a deposit address for a currency
        /// </summary>
        /// <param name="currency">The name of the currency to get request a deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address</returns>
        public async Task<WebCallResult<BittrexDepositAddressV3>> RequestDepositAddressAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency }
            };

            return await SendRequest<BittrexDepositAddressV3>(GetUrl("addresses"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region deposits
        /// <summary>
        /// Gets list of open deposits. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<IEnumerable<BittrexDepositV3>> GetOpenDeposits(string? currency = null, CancellationToken ct = default) => GetOpenDepositsAsync(currency, ct).Result;

        /// <summary>
        /// Gets list of open deposits. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetOpenDepositsAsync(string? currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);

            return await SendRequest<IEnumerable<BittrexDepositV3>>(GetUrl("deposits/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<IEnumerable<BittrexDepositV3>> GetClosedDeposits(string? currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default) =>
            GetClosedDepositsAsync(currency, status, startDate, endDate, pageSize, nextPageToken, previousPageToken, ct).Result;

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
        public async Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetClosedDepositsAsync(string? currency = null, DepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify startDate and endData simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 25, 100);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequest<IEnumerable<BittrexDepositV3>>(GetUrl("deposits/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<IEnumerable<BittrexDepositV3>> GetDepositsByTransactionId(string transactionId, CancellationToken ct = default) => GetDepositsByTransactionIdAsync(transactionId, ct).Result;

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<IEnumerable<BittrexDepositV3>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await SendRequest<IEnumerable<BittrexDepositV3>>(GetUrl($"deposits/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit info</returns>
        public WebCallResult<BittrexDepositV3> GetDeposit(string depositId, CancellationToken ct = default) => GetDepositAsync(depositId, ct).Result;

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit info</returns>
        public async Task<WebCallResult<BittrexDepositV3>> GetDepositAsync(string depositId, CancellationToken ct = default)
        {
            depositId.ValidateNotNull(nameof(depositId));
            return await SendRequest<BittrexDepositV3>(GetUrl($"deposits/{depositId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        #endregion

        #region orders
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
        public WebCallResult<IEnumerable<BittrexOrderV3>> GetClosedOrders(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default) =>
            GetClosedOrdersAsync(symbol, startDate, endDate, pageSize, nextPageToken, previousPageToken, ct).Result;

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
        public async Task<WebCallResult<IEnumerable<BittrexOrderV3>>> GetClosedOrdersAsync(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            pageSize?.ValidateIntBetween(nameof(pageSize), 1, 200);

            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 25, 100);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequest<IEnumerable<BittrexOrderV3>>(GetUrl("orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<IEnumerable<BittrexOrderV3>> GetOpenOrders(string? symbol = null, CancellationToken ct = default) => GetOpenOrdersAsync(symbol, ct).Result;

        /// <summary>
        /// Gets a list of open orders. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderV3>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await SendRequest<IEnumerable<BittrexOrderV3>>(GetUrl("orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> GetOrder(string orderId, CancellationToken ct = default) => GetOrderAsync(orderId, ct).Result;

        /// <summary>
        /// Gets info on an order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await SendRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets executions (trades) for a order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve executions for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        public WebCallResult<IEnumerable<BittrexExecution>> GetExecutions(string orderId, CancellationToken ct = default) => GetExecutionsAsync(orderId, ct).Result;

        /// <summary>
        /// Gets executions (trades) for a order
        /// </summary>
        /// <param name="orderId">The id of the order to retrieve executions for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Executions</returns>
        public async Task<WebCallResult<IEnumerable<BittrexExecution>>> GetExecutionsAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await SendRequest<IEnumerable<BittrexExecution>>(GetUrl($"orders/{orderId}/executions"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public WebCallResult<BittrexOrderV3> CancelOrder(string orderId, CancellationToken ct = default) => CancelOrderAsync(orderId, ct).Result;
        
        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">The id of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await SendRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Cancels all open orders
        /// </summary>
        /// <param name="market">Only cancel open orders for a specific market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public WebCallResult<IEnumerable<BittrexOrderV3>> CancelAllOpenOrders(string? market = null,
            CancellationToken ct = default) => CancelAllOpenOrdersAsync(market, ct).Result;

        /// <summary>
        /// Cancels all open orders
        /// </summary>
        /// <param name="market">Only cancel open orders for a specific market</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order info</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderV3>>> CancelAllOpenOrdersAsync(string? market = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", market);
            return await SendRequest<IEnumerable<BittrexOrderV3>>(GetUrl($"orders/open/"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
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
        /// <param name="useAwards">Option to use Bittrex credits for the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        public WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, TimeInForce timeInForce, decimal quantity, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default) =>
            PlaceOrderAsync(symbol, direction, type, timeInForce, quantity, limit, ceiling, clientOrderId, useAwards, ct).Result;

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
        /// <param name="useAwards">Option to use Bittrex credits for the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, TimeInForce timeInForce, decimal? quantity, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>()
            {
                {"marketSymbol", symbol},
                {"direction", JsonConvert.SerializeObject(direction, new OrderSideConverter(false))},
                {"type", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
                {"timeInForce",  JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)) }
            };
            parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientOrderId", clientOrderId);
            parameters.AddOptionalParameter("ceiling", ceiling?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("useAwards", useAwards);

            return await SendRequest<BittrexOrderV3>(GetUrl("orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region withdrawals
        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open withdrawals</returns>
        public WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetOpenWithdrawals(string? currency = null, WithdrawalStatus? status = null, CancellationToken ct = default) => GetOpenWithdrawalsAsync(currency, status, ct).Result;

        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="currency">Filter by currency</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open withdrawals</returns>
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetOpenWithdrawalsAsync(string? currency = null, WithdrawalStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status);

            return await SendRequest<IEnumerable<BittrexWithdrawalV3>>(GetUrl("withdrawals/open"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed withdrawals</returns>
        public WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetClosedWithdrawals(string? currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default) =>
            GetClosedWithdrawalsAsync(currency, status, startDate, endDate, pageSize, nextPageToken, previousPageToken, ct).Result;

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
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetClosedWithdrawalsAsync(string? currency = null, WithdrawalStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify startDate and endData simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 25, 100);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequest<IEnumerable<BittrexWithdrawalV3>>(GetUrl("withdrawals/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawals</returns>
        public WebCallResult<IEnumerable<BittrexWithdrawalV3>> GetWithdrawalsByTransactionId(string transactionId, CancellationToken ct = default) => GetWithdrawalsByTransactionIdAsync(transactionId, ct).Result;

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawals</returns>
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawalV3>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await SendRequest<IEnumerable<BittrexWithdrawalV3>>(GetUrl($"withdrawals/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> GetWithdrawal(string id, CancellationToken ct = default) => GetWithdrawalAsync(id, ct).Result;

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> GetWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await SendRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        public WebCallResult<BittrexWithdrawalV3> CancelWithdrawal(string id, CancellationToken ct = default) => CancelWithdrawalAsync(id, ct).Result;

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> CancelWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await SendRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="clientWithdrawId">Client id to identify the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        public WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string? addressTag = null, string? clientWithdrawId = null, CancellationToken ct = default) =>
            WithdrawAsync(currency, quantity, address, addressTag, clientWithdrawId, ct).Result;

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="clientWithdrawId">Client id to identify the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string? addressTag = null, string? clientWithdrawId = null, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            address.ValidateNotNull(nameof(address));
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency},
                { "quantity", quantity},
                { "cryptoAddress", address}
            };

            parameters.AddOptionalParameter("cryptoAddressTag", addressTag);
            parameters.AddOptionalParameter("clientWithdrawalId", clientWithdrawId);

            return await SendRequest<BittrexWithdrawalV3>(GetUrl("withdrawals"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of whitelisted address for withdrawals
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawal address</returns>
        public WebCallResult<IEnumerable<BittrexWhitelistAddress>> GetWithdrawalWhitelistAddresses(CancellationToken ct = default) => GetWithdrawalWhitelistAddressesAsync(ct).Result;

        /// <summary>
        /// Gets a list of whitelisted address for withdrawals
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawal address</returns>
        public async Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexWhitelistAddress>>(GetUrl($"withdrawals/whitelistAddresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region conditional orders
        /// <summary>
        /// Get details on a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        public WebCallResult<BittrexConditionalOrder> GetConditionalOrder(string? orderId = null, CancellationToken ct = default) => GetConditionalOrderAsync(orderId, ct).Result;

        /// <summary>
        /// Get details on a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        public async Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = null, CancellationToken ct = default)
        {
            return await SendRequest<BittrexConditionalOrder>(GetUrl($"conditional-orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        public WebCallResult<BittrexConditionalOrder> CancelConditionalOrder(string? orderId = null, CancellationToken ct = default) => CancelConditionalOrderAsync(orderId, ct).Result;

        /// <summary>
        /// Cancels a condtional order
        /// </summary>
        /// <param name="orderId">Id of the conditional order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional order</returns>
        public async Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string? orderId = null, CancellationToken ct = default)
        {
            return await SendRequest<BittrexConditionalOrder>(GetUrl($"conditional-orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

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
        public WebCallResult<IEnumerable<BittrexConditionalOrder>> GetClosedConditionalOrders(string? symbol = null, DateTime? startDate = null,
            DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
            => GetClosedConditionalOrdersAsync(symbol, startDate, endDate, pageSize, nextPageToken, previousPageToken, ct).Result;

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
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = null, DateTime? startDate = null, DateTime? endDate = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 1, 200);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequest<IEnumerable<BittrexConditionalOrder>>(GetUrl("conditional-orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get list op open conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional orders</returns>
        public WebCallResult<IEnumerable<BittrexConditionalOrder>> GetOpenConditionalOrders(string? symbol = null, CancellationToken ct = default) => GetOpenConditionalOrdersAsync(symbol, ct).Result;

        /// <summary>
        /// Get list op open conditional orders
        /// </summary>
        /// <param name="symbol">Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Conditional orders</returns>
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);
            return await SendRequest<IEnumerable<BittrexConditionalOrder>>(GetUrl($"conditional-orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

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
        public WebCallResult<BittrexConditionalOrder> PlaceConditionalOrder(string symbol,
            BittrexConditionalOrderOperand operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default) =>
            PlaceConditionalOrderAsync(symbol, operand, orderToCreate, orderToCancel, triggerPrice, trailingStopPercent, clientConditionalOrderId).Result;

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
        public async Task<WebCallResult<BittrexConditionalOrder>> PlaceConditionalOrderAsync(
            string symbol,
            BittrexConditionalOrderOperand operand,
            BittrexUnplacedOrder? orderToCreate = null,
            BittrexLinkedOrder? orderToCancel = null,
            decimal? triggerPrice = null,
            decimal? trailingStopPercent = null,
            string? clientConditionalOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "marketSymbol", symbol },
                { "operand", JsonConvert.SerializeObject(operand, new BittrexConditionalOrderOperandConverter(false)) }
            };

            parameters.AddOptionalParameter("triggerPrice", triggerPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopPercent", trailingStopPercent?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientConditionalOrderId", clientConditionalOrderId);
            parameters.AddOptionalParameter("orderToCreate", orderToCreate);
            parameters.AddOptionalParameter("orderToCancel", orderToCancel);

            return await SendRequest<BittrexConditionalOrder>(GetUrl($"conditional-orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken data)
        {
            if (data["code"] == null)
                return new UnknownError("Unknown response from server", data);

            var info = (string)data["code"];
            if (data["detail"] != null)
                info += "; Details: " + (string)data["detail"];
            if (data["data"] != null)
                info += "; Data: " + data["data"];

            return new ServerError(info);
        }

        /// <summary>
        /// Get url for an endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        protected Uri GetUrl(string endpoint)
        {
            return new Uri($"{BaseAddress}v3/{endpoint}");
        }
        #endregion
    }
}
