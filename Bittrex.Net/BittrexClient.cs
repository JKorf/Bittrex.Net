using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
    /// Client for the Bittrex Rest API. Consider using the BittrexClientV3 client as this client (V1 API) has end of life planned ib 9/30/2020
    /// </summary>
    public class BittrexClient: RestClient, IBittrexClient
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();
        private static BittrexClientOptions DefaultOptions => defaultOptions.Copy();

        private const string Api = "api";
        private const string ApiVersion = "1.1";
        private const string ApiVersion2 = "2.0";
        private readonly string baseAddressV2;

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
        public BittrexClient(BittrexClientOptions options): base("Bittrex", options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            baseAddressV2 = options.BaseAddressV2;
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
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        public WebCallResult<IEnumerable<BittrexSymbol>> GetSymbols(CancellationToken ct = default) => GetSymbolsAsync(ct).Result;

        /// <summary>
        /// Gets information about all available symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        public async Task<WebCallResult<IEnumerable<BittrexSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            return await Execute<IEnumerable<BittrexSymbol>>(GetUrl(MarketsEndpoint, Api, ApiVersion), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        public WebCallResult<IEnumerable<BittrexCurrency>> GetCurrencies(CancellationToken ct = default) => GetCurrenciesAsync(ct).Result;

        /// <summary>
        /// Gets information about all available currencies
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of currencies</returns>
        public async Task<WebCallResult<IEnumerable<BittrexCurrency>>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            return await Execute<IEnumerable<BittrexCurrency>>(GetUrl(CurrenciesEndpoint, Api, ApiVersion), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the price of a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get price for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The current ask, bid and last prices for the symbol</returns>
        public WebCallResult<BittrexPrice> GetTicker(string symbol, CancellationToken ct = default) => GetTickerAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the price of a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get price for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The current ask, bid and last prices for the symbol</returns>
        public async Task<WebCallResult<BittrexPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol }
            };

            return await Execute<BittrexPrice>(GetUrl(TickerEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a summary of the symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List with single entry containing info for the symbol</returns>
        public WebCallResult<BittrexSymbolSummary> GetSymbolSummary(string symbol, CancellationToken ct = default) => GetSymbolSummaryAsync(symbol, ct).Result;

        /// <summary>
        /// Gets a summary of the symbol
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List with single entry containing info for the symbol</returns>
        public async Task<WebCallResult<BittrexSymbolSummary>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol }
            };

            var result = await Execute<IEnumerable<BittrexSymbolSummary>>(GetUrl(MarketSummaryEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
            return new WebCallResult<BittrexSymbolSummary>(result.ResponseStatusCode, result.ResponseHeaders, result.Data?.Any() == true ? result.Data.First(): null, result.Error);
        }

        /// <summary>
        /// Gets a summary for all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of summaries for all symbols</returns>
        public WebCallResult<IEnumerable<BittrexSymbolSummary>> GetSymbolSummaries(CancellationToken ct = default) => GetSymbolSummariesAsync(ct).Result;

        /// <summary>
        /// Gets a summary for all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of summaries for all symbols</returns>
        public async Task<WebCallResult<IEnumerable<BittrexSymbolSummary>>> GetSymbolSummariesAsync(CancellationToken ct = default)
        {
            return await Execute<IEnumerable<BittrexSymbolSummary>>(GetUrl(MarketSummariesEndpoint, Api, ApiVersion), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with bids and asks for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public WebCallResult<BittrexOrderBook> GetOrderBook(string symbol, CancellationToken ct = default) => GetOrderBookAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the order book with bids and asks for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public async Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol },
                { "type", "both" }
            };

            return await Execute<BittrexOrderBook>(GetUrl(OrderBookEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with bids for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public WebCallResult<IEnumerable<BittrexOrderBookEntry>> GetBuyOrderBook(string symbol, CancellationToken ct = default) => GetBuyOrderBookAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the order book with bids for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderBookEntry>>> GetBuyOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol },
                { "type", "buy" }
            };

            return await Execute<IEnumerable<BittrexOrderBookEntry>>(GetUrl(OrderBookEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book with asks for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public WebCallResult<IEnumerable<BittrexOrderBookEntry>> GetSellOrderBook(string symbol, CancellationToken ct = default) => GetSellOrderBookAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the order book with asks for a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get the order book for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Order book for the symbol</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderBookEntry>>> GetSellOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol },
                { "type", "sell" }
            };

            return await Execute<IEnumerable<BittrexOrderBookEntry>>(GetUrl(OrderBookEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the last trades on a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get history for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of trade aggregations</returns>
        public WebCallResult<IEnumerable<BittrexSymbolTrade>> GetSymbolTrades(string symbol, CancellationToken ct = default) => GetSymbolTradesAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the last trades on a symbol
        /// </summary>
        /// <param name="symbol">Symbol to get history for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of trade aggregations</returns>
        public async Task<WebCallResult<IEnumerable<BittrexSymbolTrade>>> GetSymbolTradesAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol }
            };

            return await Execute<IEnumerable<BittrexSymbolTrade>>(GetUrl(MarketHistoryEndpoint, Api, ApiVersion), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets kline data for a symbol on a specific interval
        /// </summary>
        /// <param name="symbol">Symbol to get kline for</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of klines</returns>
        public WebCallResult<IEnumerable<BittrexKline>> GetKlines(string symbol, TickInterval interval, CancellationToken ct = default) => GetKlinesAsync(symbol, interval, ct).Result;

        /// <summary>
        /// Gets kline data for a symbol on a specific interval
        /// </summary>
        /// <param name="symbol">Symbol to get kline for</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of klines</returns>
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetKlinesAsync(string symbol, TickInterval interval, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "marketName", symbol },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<IEnumerable<BittrexKline>>(GetUrl(CandleEndpoint, Api, ApiVersion2), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets kline data for a symbol on a specific interval
        /// </summary>
        /// <param name="symbol">Symbol to get klines for</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of klines</returns>
        public WebCallResult<IEnumerable<BittrexKline>> GetLastKline(string symbol, TickInterval interval, CancellationToken ct = default) => GetLastKlineAsync(symbol, interval, ct).Result;

        /// <summary>
        /// Gets kline data for a symbol on a specific interval
        /// </summary>
        /// <param name="symbol">Symbol to get klines for</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of klines</returns>
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetLastKlineAsync(string symbol, TickInterval interval, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "marketName", symbol },
                { "tickInterval", JsonConvert.SerializeObject(interval, new TickIntervalConverter(false)) }
            };

            return await Execute<IEnumerable<BittrexKline>>(GetUrl(LatestCandleEndpoint, Api, ApiVersion2), HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="symbol">Symbol to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public WebCallResult<BittrexGuid> PlaceOrder(OrderSide side, string symbol, decimal quantity, decimal rate, CancellationToken ct = default) =>
            PlaceOrderAsync(side, symbol, quantity, rate, ct).Result;

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="side">Side of the order</param>
        /// <param name="symbol">Symbol to place the order on</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="rate">The rate per unit of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string symbol, decimal quantity, decimal rate, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "rate", rate.ToString(CultureInfo.InvariantCulture) }
            };

            var uri = GetUrl(side == OrderSide.Buy ? BuyLimitEndpoint : SellLimitEndpoint, Api, ApiVersion);
            return await Execute<BittrexGuid>(uri, HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public WebCallResult<object> CancelOrder(Guid guid, CancellationToken ct = default) => CancelOrderAsync(guid, ct).Result;

        /// <summary>
        /// Cancels an open order
        /// </summary>
        /// <param name="guid">The Guid of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        public async Task<WebCallResult<object>> CancelOrderAsync(Guid guid, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };

            return await Execute<object>(GetUrl(CancelEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">Filter list by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public WebCallResult<IEnumerable<BittrexOpenOrdersOrder>> GetOpenOrders(string? symbol = null, CancellationToken ct = default) => GetOpenOrdersAsync(symbol, ct).Result;

        /// <summary>
        /// Gets a list of open orders
        /// </summary>
        /// <param name="symbol">Filter list by symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOpenOrdersOrder>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            symbol?.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", symbol);

            return await Execute<IEnumerable<BittrexOpenOrdersOrder>>(GetUrl(OpenOrdersEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The balance of the currency</returns>
        public WebCallResult<BittrexBalance> GetBalance(string currency, CancellationToken ct = default) => GetBalanceAsync(currency, ct).Result;

        /// <summary>
        /// Gets the balance of a single currency
        /// </summary>
        /// <param name="currency">Currency to get the balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The balance of the currency</returns>
        public async Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));

            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexBalance>(GetUrl(BalanceEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        public WebCallResult<IEnumerable<BittrexBalance>> GetBalances(CancellationToken ct = default) => GetBalancesAsync(ct).Result;

        /// <summary>
        /// Gets a list of all balances for the current account
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        public async Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await Execute<IEnumerable<BittrexBalance>>(GetUrl(BalancesEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address of the currency</returns>
        public WebCallResult<BittrexDepositAddress> GetDepositAddress(string currency, CancellationToken ct = default) => GetDepositAddressAsync(currency, ct).Result;

        /// <summary>
        /// Gets the deposit address for a specific currency
        /// </summary>
        /// <param name="currency">Currency to get deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address of the currency</returns>
        public async Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency}
            };
            return await Execute<BittrexDepositAddress>(GetUrl(DepositAddressEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Guid of the withdrawal</returns>
        public WebCallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string? paymentId = null, CancellationToken ct = default) => 
            WithdrawAsync(currency, quantity, address, paymentId, ct).Result;

        /// <summary>
        /// Places a withdraw request on Bittrex
        /// </summary>
        /// <param name="currency">The currency to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="paymentId">Optional string identifier to add to the withdraw</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Guid of the withdrawal</returns>
        public async Task<WebCallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string? paymentId = null, CancellationToken ct = default)
        {
            currency.ValidateNotNull(nameof(currency));
            var parameters = new Dictionary<string, object>
            {
                {"currency", currency},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"address", address}
            };
            parameters.AddOptionalParameter("paymentid", paymentId);

            return await Execute<BittrexGuid>(GetUrl(WithdrawEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The requested order</returns>
        public WebCallResult<BittrexAccountOrder> GetOrder(Guid guid, CancellationToken ct = default) => GetOrderAsync(guid, ct).Result;

        /// <summary>
        /// Gets an order by it's guid
        /// </summary>
        /// <param name="guid">The guid of the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The requested order</returns>
        public async Task<WebCallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"uuid", guid.ToString()}
            };
            return await Execute<BittrexAccountOrder>(GetUrl(OrderEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="symbol">Filter on symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of orders</returns>
        public WebCallResult<IEnumerable<BittrexOrderHistoryOrder>> GetOrderHistory(string? symbol = null, CancellationToken ct = default) => GetOrderHistoryAsync(symbol, ct).Result;

        /// <summary>
        /// Gets the order history for the current account
        /// </summary>
        /// <param name="symbol">Filter on symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of orders</returns>
        public async Task<WebCallResult<IEnumerable<BittrexOrderHistoryOrder>>> GetOrderHistoryAsync(string? symbol = null, CancellationToken ct = default)
        {
            symbol?.ValidateBittrexSymbol();
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("market", symbol);
            return await Execute<IEnumerable<BittrexOrderHistoryOrder>>(GetUrl(OrderHistoryEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdrawals</returns>
        public WebCallResult<IEnumerable<BittrexWithdrawal>> GetWithdrawalHistory(string? currency = null, CancellationToken ct = default) => GetWithdrawalHistoryAsync(currency, ct).Result;

        /// <summary>
        /// Gets the withdrawal history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdrawals</returns>
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalHistoryAsync(string? currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<IEnumerable<BittrexWithdrawal>>(GetUrl(WithdrawalHistoryEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public WebCallResult<IEnumerable<BittrexDeposit>> GetDepositHistory(string? currency = null, CancellationToken ct = default) => GetDepositHistoryAsync(currency, ct).Result;

        /// <summary>
        /// Gets the deposit history of the current account
        /// </summary>
        /// <param name="currency">Filter on currency</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositHistoryAsync(string? currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currency", currency);
            return await Execute<IEnumerable<BittrexDeposit>>(GetUrl(DepositHistoryEndpoint, Api, ApiVersion), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
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

            var result = $"{address}{api}/v{version}/{endpoint}";
            return new Uri(result);
        }

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken data)
        {
            if(!data.HasValues || data["message"] == null)
                return new UnknownError("Unknown response from server", data);

            return new ServerError((string)data["message"]);
        }

        private async Task<WebCallResult<T>> Execute<T>(Uri uri, HttpMethod method, CancellationToken ct, bool signed = false, Dictionary<string, object>? parameters = null) where T: class
        {
            return GetResult(await SendRequest<BittrexApiResult<T>>(uri, method, ct, parameters, signed).ConfigureAwait(false));
        }
        
        private static WebCallResult<T> GetResult<T>(WebCallResult<BittrexApiResult<T>> result) where T : class
        {
            if (result.Error != null || result.Data == null)
                return WebCallResult<T>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error!);

            var messageEmpty = string.IsNullOrEmpty(result.Data.Message);
            return new WebCallResult<T>(result.ResponseStatusCode, result.ResponseHeaders, !messageEmpty ? null: result.Data.Result, !messageEmpty ? new ServerError(result.Data.Message ?? "-"): null);
        }

        #endregion
        #endregion
    }
}
