﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
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


        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        public WebCallResult<IEnumerable<BittrexMarketV3>> GetMarkets(CancellationToken ct = default) => GetMarketsAsync(ct).Result;

        /// <summary>
        /// Gets information about all available markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        public async Task<WebCallResult<IEnumerable<BittrexMarketV3>>> GetMarketsAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexMarketV3>>(GetUrl("markets"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of markets</returns>
        public WebCallResult<BittrexMarketV3> GetMarket(string market, CancellationToken ct = default) => GetMarketAsync(market, ct).Result;

        /// <summary>
        /// Gets information about a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market info</returns>
        public async Task<WebCallResult<BittrexMarketV3>> GetMarketAsync(string market, CancellationToken ct = default)
        {
            return await SendRequest<BittrexMarketV3>(GetUrl("markets/" + market), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of market summaries</returns>
        public WebCallResult<IEnumerable<BittrexMarketSummariesV3>> GetMarketSummaries(CancellationToken ct = default) => GetMarketSummariesAsync(ct).Result;

        /// <summary>
        /// Gets summaries of all markets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of market summaries</returns>
        public async Task<WebCallResult<IEnumerable<BittrexMarketSummariesV3>>> GetMarketSummariesAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexMarketSummariesV3>>(GetUrl("markets/summaries"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market summary</returns>
        public WebCallResult<BittrexMarketSummariesV3> GetMarketSummary(string market, CancellationToken ct = default) => GetMarketSummaryAsync(market, ct).Result;

        /// <summary>
        /// Gets summary of a market
        /// </summary>
        /// <param name="market">The market to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market summary</returns>
        public async Task<WebCallResult<BittrexMarketSummariesV3>> GetMarketSummaryAsync(string market, CancellationToken ct = default)
        {
            return await SendRequest<BittrexMarketSummariesV3>(GetUrl($"markets/{market}/summary"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market order book</returns>
        public WebCallResult<BittrexMarketOrderBookV3> GetMarketOrderBook(string market, CancellationToken ct = default) => GetMarketOrderBookAsync(market, ct).Result;

        /// <summary>
        /// Gets the order book of a market
        /// </summary>
        /// <param name="market">The market to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market order book</returns>
        public async Task<WebCallResult<BittrexMarketOrderBookV3>> GetMarketOrderBookAsync(string market, CancellationToken ct = default)
        {
            return await SendRequest<BittrexMarketOrderBookV3>(GetUrl($"markets/{market}/orderbook"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market trade list</returns>
        public WebCallResult<IEnumerable<BittrexMarketTradeV3>> GetMarketTrades(string market, CancellationToken ct = default) => GetMarketTradesAsync(market, ct).Result;

        /// <summary>
        /// Gets the trade history of a market
        /// </summary>
        /// <param name="market">The market to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market trade list</returns>
        public async Task<WebCallResult<IEnumerable<BittrexMarketTradeV3>>> GetMarketTradesAsync(string market, CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexMarketTradeV3>>(GetUrl($"markets/{market}/trades"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market ticker</returns>
        public WebCallResult<BittrexMarketTickV3> GetMarketTicker(string market, CancellationToken ct = default) => GetMarketTickerAsync(market, ct).Result;

        /// <summary>
        /// Gets the ticker of a market
        /// </summary>
        /// <param name="market">The market to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market ticker</returns>
        public async Task<WebCallResult<BittrexMarketTickV3>> GetMarketTickerAsync(string market, CancellationToken ct = default)
        {
            var result = await SendRequest<BittrexMarketTickV3>(GetUrl($"markets/{market}/ticker"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (result.Success)
                result.Data.Symbol = market;
            return result;
        }

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market tickers</returns>
        public WebCallResult<IEnumerable<BittrexMarketTickV3>> GetMarketTickers(CancellationToken ct = default) => GetMarketTickersAsync(ct).Result;

        /// <summary>
        /// Gets list of tickers for all market
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market tickers</returns>
        public async Task<WebCallResult<IEnumerable<BittrexMarketTickV3>>> GetMarketTickersAsync(CancellationToken ct = default)
        {
            return await SendRequest<IEnumerable<BittrexMarketTickV3>>(GetUrl("markets/tickers"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market candles</returns>
        public WebCallResult<IEnumerable<BittrexMarketCandleV3>> GetMarketCandles(string market, CandleInterval interval, CancellationToken ct = default) => GetMarketCandlesAsync(market, interval, ct).Result;

        /// <summary>
        /// Gets the candles for a market
        /// </summary>
        /// <param name="market">The market to get candles for</param>
        /// <param name="interval">The interval of the candles</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Market candles</returns>
        public async Task<WebCallResult<IEnumerable<BittrexMarketCandleV3>>> GetMarketCandlesAsync(string market, CandleInterval interval, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"candleInterval", JsonConvert.SerializeObject(interval, new CandleIntervalConverter(false))}
            };

            return await SendRequest<IEnumerable<BittrexMarketCandleV3>>(GetUrl($"markets/{market}/candles"), HttpMethod.Get, ct, parameters: parameters).ConfigureAwait(false);
        }

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
            return await SendRequest<BittrexCurrencyV3>(GetUrl($"currencies/{currency}"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets current balances
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        public WebCallResult<IEnumerable<BittrexBalanceV3>> GetBalances(CancellationToken ct = default) => GetBalancesAsync(ct).Result;

        /// <summary>
        /// Gets current balances
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
        /// <returns>Balance for market</returns>
        public WebCallResult<BittrexBalanceV3> GetBalance(string currency, CancellationToken ct = default) => GetBalanceAsync(currency, ct).Result;

        /// <summary>
        /// Gets current balance for a market
        /// </summary>
        /// <param name="currency">The name of the currency to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for market</returns>
        public async Task<WebCallResult<BittrexBalanceV3>> GetBalanceAsync(string currency, CancellationToken ct = default)
        {
            return await SendRequest<BittrexBalanceV3>(GetUrl($"balances/{currency}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

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
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency }
            };

            return await SendRequest<BittrexDepositAddressV3>(GetUrl("addresses"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of open deposits
        /// </summary>
        /// <param name="currency">Filter the list by currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<IEnumerable<BittrexDepositV3>> GetOpenDeposits(string? currency = null, CancellationToken ct = default) => GetOpenDepositsAsync(currency, ct).Result;

        /// <summary>
        /// Gets list of open deposits
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
                return WebCallResult<IEnumerable<BittrexDepositV3>>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<IEnumerable<BittrexDepositV3>>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)): null);
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
            return await SendRequest<BittrexDepositV3>(GetUrl($"deposits/{depositId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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
            if (nextPageToken != null && previousPageToken != null)
                return WebCallResult<IEnumerable<BittrexOrderV3>>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<IEnumerable<BittrexOrderV3>>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

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
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<IEnumerable<BittrexOrderV3>> GetOpenOrders(string? symbol = null, CancellationToken ct = default) => GetOpenOrdersAsync(symbol, ct).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">The symbol to get open orders for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderV3>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await SendRequest< IEnumerable<BittrexOrderV3>>(GetUrl("orders/open"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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
            return await SendRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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
            return await SendRequest<BittrexOrderV3>(GetUrl($"orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
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
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        public WebCallResult<BittrexOrderV3> PlaceOrder(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity,  TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, CancellationToken ct = default) => 
            PlaceOrderAsync(symbol, direction, type, quantity, timeInForce, limit, ceiling, clientOrderId, ct).Result;

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
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order info</returns>
        public async Task<WebCallResult<BittrexOrderV3>> PlaceOrderAsync(string symbol, OrderSide direction, OrderTypeV3 type, decimal quantity, TimeInForce timeInForce, decimal? limit = null, decimal? ceiling = null, string? clientOrderId = null, CancellationToken ct = default)
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

            return await SendRequest<BittrexOrderV3>(GetUrl("orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

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

            return await SendRequest<IEnumerable<BittrexWithdrawalV3>>(GetUrl($"withdrawals/open"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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
                return WebCallResult<IEnumerable<BittrexWithdrawalV3>>.CreateErrorResult(new ArgumentError("Can't specify startDate and endData simultaneously"));

            if (pageSize < 1 || pageSize > 200)
                return WebCallResult<IEnumerable<BittrexWithdrawalV3>>.CreateErrorResult(new ArgumentError("Page size should be between 1 and 200"));

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", currency);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequest<IEnumerable<BittrexWithdrawalV3>>(GetUrl($"withdrawals/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            return await SendRequest<BittrexWithdrawalV3>(GetUrl($"withdrawals/{id}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        public WebCallResult<BittrexWithdrawalV3> Withdraw(string currency, decimal quantity, string address, string addressTag, CancellationToken ct = default) =>
            WithdrawAsync(currency, quantity, address, addressTag, ct).Result;

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        public async Task<WebCallResult<BittrexWithdrawalV3>> WithdrawAsync(string currency, decimal quantity, string address, string addressTag, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", currency},
                { "quantity", quantity},
                { "cryptoAddress", address},
                { "cryptoAddressTag", addressTag},
            };

            return await SendRequest<BittrexWithdrawalV3>(GetUrl("withdrawals"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken data)
        {
            if (data["code"] == null)
                return new UnknownError("Unknown response from server: " + data);

            var info = (string)data["code"];
            if (data["detail"] != null)
                info += "; Details: " + (string) data["detail"];
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
            return new Uri($"{BaseAddress}/v3/{endpoint}");
        }
        #endregion
    }
}
