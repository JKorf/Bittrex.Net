using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex V3 API
    /// </summary>
    public class BittrexClient : RestClient, IExchangeClient, IBittrexClient
    {
        #region fields
        private static BittrexClientOptions defaultOptions = new BittrexClientOptions();

        private static BittrexClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        /// <summary>
        /// Event triggered when an order is placed via this client
        /// </summary>
        public event Action<ICommonOrderId>? OnOrderPlaced;
        /// <summary>
        /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOpenOrdersAsync
        /// </summary>
        public event Action<ICommonOrderId>? OnOrderCanceled;

        #region ctor
        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// </summary>
        public BittrexClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// </summary>
        public BittrexClient(BittrexClientOptions options) : base("Bittrex", options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
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
            SetAuthenticationProvider(new BittrexAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<BittrexServerTime>(GetUrl("ping"), HttpMethod.Get, ct).ConfigureAwait(false);
            return result.As<DateTime>(result.Data?.ServerTime ?? default);
        }

        #region symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexSymbol>>(GetUrl("markets"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            return await SendRequestAsync<IEnumerable<BittrexAssetPermission>>(GetUrl("account/permissions/currencies/" + asset), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexAssetPermission>>(GetUrl("account/permissions/currencies"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateNotNull(nameof(symbol));
            symbol.ValidateBittrexSymbol();

            return await SendRequestAsync<IEnumerable<BittrexSymbolPermission>>(GetUrl("account/permissions/markets/" + symbol), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexSymbolPermission>>(GetUrl("account/permissions/markets"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequestAsync<BittrexSymbol>(GetUrl("markets/" + symbol), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolSummary>>> GetSymbolSummariesAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexSymbolSummary>>(GetUrl("markets/summaries"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexSymbolSummary>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequestAsync<BittrexSymbolSummary>(GetUrl($"markets/{symbol}/summary"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            limit?.ValidateIntValues(nameof(limit), 1, 25, 500);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("depth", limit?.ToString(CultureInfo.InvariantCulture));

            var result = await SendRequestAsync<BittrexOrderBook>(GetUrl($"markets/{symbol}/orderbook"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (result.Data != null)
                result.Data.Sequence = result.ResponseHeaders!.GetSequence() ?? 0;
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTrade>>> GetTradeHistoryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await SendRequestAsync<IEnumerable<BittrexTrade>>(GetUrl($"markets/{symbol}/trades"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var result = await SendRequestAsync<BittrexTick>(GetUrl($"markets/{symbol}/ticker"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (result.Success && string.IsNullOrEmpty(result.Data.Symbol))
                result.Data.Symbol = symbol;
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTick>>> GetTickersAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexTick>>(GetUrl("markets/tickers"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetKlinesAsync(string symbol, KlineInterval interval, KlineType? type = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            return await SendRequestAsync<IEnumerable<BittrexKline>>(GetUrl($"markets/{symbol}/candles{(type.HasValue ? "/" + type.ToString().ToUpperInvariant() : "")}/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/recent"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, KlineType? type = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            if (interval == KlineInterval.OneDay && month.HasValue)
                throw new ArgumentException("Can't specify month value when using day interval");

            if (interval == KlineInterval.OneHour && day.HasValue)
                throw new ArgumentException("Can't specify day value when using hour interval");

            if (day.HasValue && !month.HasValue)
                throw new ArgumentException("Can't specify day value without month value");

            var url =
                $"markets/{symbol}/candles{(type.HasValue ? "/" + type.ToString().ToUpperInvariant() : "")}/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/historical/{year}";
            if (month.HasValue)
                url += "/" + month;
            if (day.HasValue)
                url += "/" + day;

            return await SendRequestAsync<IEnumerable<BittrexKline>>(GetUrl(url), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        #endregion

        #region currencies

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAsset>>> GetAssetsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexAsset>>(GetUrl("currencies"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAsset>> GetAssetAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await SendRequestAsync<BittrexAsset>(GetUrl($"currencies/{asset}"), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        #endregion

        #region accounts

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<BittrexAccount>(GetUrl("account"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTradingFee>>> GetTradingFeesAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexTradingFee>>(GetUrl("account/fees/trading"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<BittrexAccountVolume>(GetUrl("account/volume"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexBalance>>(GetUrl("balances"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await SendRequestAsync<BittrexBalance>(GetUrl($"balances/{asset}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region addresses

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDepositAddress>>> GetDepositAddressesAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexDepositAddress>>(GetUrl("addresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await SendRequestAsync<BittrexDepositAddress>(GetUrl($"addresses/{asset}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDepositAddress>> RequestDepositAddressAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", asset }
            };

            return await SendRequestAsync<BittrexDepositAddress>(GetUrl("addresses"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region deposits

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetOpenDepositsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);

            return await SendRequestAsync<IEnumerable<BittrexDeposit>>(GetUrl("deposits/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetClosedDepositsAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            pageSize?.ValidateIntBetween(nameof(pageSize), 1, 200);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequestAsync<IEnumerable<BittrexDeposit>>(GetUrl("deposits/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await SendRequestAsync<IEnumerable<BittrexDeposit>>(GetUrl($"deposits/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDeposit>> GetDepositAsync(string depositId, CancellationToken ct = default)
        {
            depositId.ValidateNotNull(nameof(depositId));
            return await SendRequestAsync<BittrexDeposit>(GetUrl($"deposits/{depositId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        #endregion

        #region orders

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> GetClosedOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            pageSize?.ValidateIntBetween(nameof(pageSize), 1, 200);

            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequestAsync<IEnumerable<BittrexOrder>>(GetUrl("orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);

            return await SendRequestAsync<IEnumerable<BittrexOrder>>(GetUrl("orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> GetOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await SendRequestAsync<BittrexOrder>(GetUrl($"orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexUserTrade>> GetUserTradeByIdAsync(string tradeId, CancellationToken ct = default)
        {
            tradeId.ValidateNotNull(nameof(tradeId));
            return await SendRequestAsync<BittrexUserTrade>(GetUrl($"executions/{tradeId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetUserTradesAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            symbol?.ValidateBittrexSymbol();

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequestAsync<IEnumerable<BittrexUserTrade>>(GetUrl($"executions"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexUserTrade>>> GetOrderTradesAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            return await SendRequestAsync<IEnumerable<BittrexUserTrade>>(GetUrl($"orders/{orderId}/executions"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            orderId.ValidateNotNull(nameof(orderId));
            var result = await SendRequestAsync<BittrexOrder>(GetUrl($"orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
            if (result)
                OnOrderCanceled?.Invoke(result.Data);
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexOrder>>> CancelAllOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            return await SendRequestAsync<IEnumerable<BittrexOrder>>(GetUrl($"orders/open/"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, TimeInForce timeInForce, decimal? quantity, decimal? price = null, decimal? quoteQuantity = null, string? clientOrderId = null, bool? useAwards = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            if ((quantity != null && quoteQuantity != null) || (quantity == null && quoteQuantity == null))
                throw new ArgumentException("Specify one of either quantity or quoteQantity");

            string orderType = JsonConvert.SerializeObject(type, new OrderTypeConverter(false));
            if (quoteQuantity != null)
                orderType = type == OrderType.Limit ? "CEILING_LIMIT" : "CEILING_MARKET";            

            var parameters = new Dictionary<string, object>()
            {
                {"marketSymbol", symbol},
                {"direction", JsonConvert.SerializeObject(side, new OrderSideConverter(false))},
                {"type", orderType },
                {"timeInForce",  JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)) }
            };
            parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientOrderId", clientOrderId);
            parameters.AddOptionalParameter("ceiling", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("useAwards", useAwards);

            var result = await SendRequestAsync<BittrexOrder>(GetUrl("orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
                OnOrderPlaced?.Invoke(result.Data);
            return result;
        }

        #endregion

        #region withdrawals

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetOpenWithdrawalsAsync(string? asset = null, WithdrawalStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);
            parameters.AddOptionalParameter("status", status);

            return await SendRequestAsync<IEnumerable<BittrexWithdrawal>>(GetUrl("withdrawals/open"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetClosedWithdrawalsAsync(string? asset = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify startTime and endTime simultaneously");

            pageSize?.ValidateIntBetween(nameof(pageSize), 1, 200);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);
            parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequestAsync<IEnumerable<BittrexWithdrawal>>(GetUrl("withdrawals/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await SendRequestAsync<IEnumerable<BittrexWithdrawal>>(GetUrl($"withdrawals/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexWithdrawal>> GetWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await SendRequestAsync<BittrexWithdrawal>(GetUrl($"withdrawals/{id}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexWithdrawal>> CancelWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await SendRequestAsync<BittrexWithdrawal>(GetUrl($"withdrawals/{id}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexWithdrawal>> WithdrawAsync(string asset, decimal quantity, string address, string? addressTag = null, string? clientWithdrawId = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            address.ValidateNotNull(nameof(address));
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", asset},
                { "quantity", quantity},
                { "cryptoAddress", address}
            };

            parameters.AddOptionalParameter("cryptoAddressTag", addressTag);
            parameters.AddOptionalParameter("clientWithdrawalId", clientWithdrawId);

            return await SendRequestAsync<BittrexWithdrawal>(GetUrl("withdrawals"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<IEnumerable<BittrexWhitelistAddress>>(GetUrl($"withdrawals/whitelistAddresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region conditional orders

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> GetConditionalOrderAsync(string? orderId = null, CancellationToken ct = default)
        {
            return await SendRequestAsync<BittrexConditionalOrder>(GetUrl($"conditional-orders/{orderId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> CancelConditionalOrderAsync(string orderId, CancellationToken ct = default)
        {
            return await SendRequestAsync<BittrexConditionalOrder>(GetUrl($"conditional-orders/{orderId}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetClosedConditionalOrdersAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default)
        {
            if (nextPageToken != null && previousPageToken != null)
                throw new ArgumentException("Can't specify nextPageToken and previousPageToken simultaneously");

            pageSize?.ValidateIntBetween("pageSize", 1, 200);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("marketSymbol", symbol);
            parameters.AddOptionalParameter("startDate", startTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("endDate", endTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            parameters.AddOptionalParameter("pageSize", pageSize);
            parameters.AddOptionalParameter("nextPageToken", nextPageToken);
            parameters.AddOptionalParameter("previousPageToken", previousPageToken);

            return await SendRequestAsync<IEnumerable<BittrexConditionalOrder>>(GetUrl("conditional-orders/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexConditionalOrder>>> GetOpenConditionalOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);
            return await SendRequestAsync<IEnumerable<BittrexConditionalOrder>>(GetUrl($"conditional-orders/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexConditionalOrder>> PlaceConditionalOrderAsync(
            string symbol,
            ConditionalOrderOperand operand,
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
                { "operand", JsonConvert.SerializeObject(operand, new ConditionalOrderOperandConverter(false)) }
            };

            parameters.AddOptionalParameter("triggerPrice", triggerPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopPercent", trailingStopPercent?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientConditionalOrderId", clientConditionalOrderId);
            parameters.AddOptionalParameter("orderToCreate", orderToCreate);
            parameters.AddOptionalParameter("orderToCancel", orderToCancel);

            return await SendRequestAsync<BittrexConditionalOrder>(GetUrl($"conditional-orders"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region batch

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> PlaceMultipleOrdersAsync(BittrexNewBatchOrder[] orders, CancellationToken ct = default)
        {
            orders?.ValidateNotNull(nameof(orders));
            if (!orders.Any())
                throw new ArgumentException("No orders provided");

            var wrapper = new Dictionary<string, object>();
            var orderParameters = new Dictionary<string, object>[orders!.Length];
            int i = 0;
            foreach (var order in orders)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("resource", "ORDER");
                parameters.Add("operation", "POST");
                var orderParameter = new Dictionary<string, object>()
                {
                    {"marketSymbol", order.Symbol},
                    {"direction", JsonConvert.SerializeObject(order.Side, new OrderSideConverter(false))},
                    {"type", JsonConvert.SerializeObject(order.Type, new OrderTypeConverter(false)) },
                    {"timeInForce",  JsonConvert.SerializeObject(order.TimeInForce, new TimeInForceConverter(false)) }
                };
                orderParameter.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture)); ;
                orderParameter.AddOptionalParameter("limit", order.Price?.ToString(CultureInfo.InvariantCulture));
                orderParameter.AddOptionalParameter("clientOrderId", order.ClientOrderId);
                orderParameter.AddOptionalParameter("ceiling", order.Ceiling?.ToString(CultureInfo.InvariantCulture));
                orderParameter.AddOptionalParameter("useAwards", order.UseAwards);
                parameters.Add("payload", orderParameter);
                orderParameters[i] = parameters;
                i++;
            }
            wrapper.Add(string.Empty, orderParameters);

            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BatchResultConverter<BittrexOrder>()
                }
            });

            return await SendRequestAsync<IEnumerable<CallResult<BittrexOrder>>>(GetUrl("batch"), HttpMethod.Post, ct, wrapper, signed: true, deserializer: serializer).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<CallResult<BittrexOrder>>>> CancelMultipleOrdersAsync(string[] ordersToCancel, CancellationToken ct = default)
        {
            ordersToCancel?.ValidateNotNull(nameof(ordersToCancel));
            if (!ordersToCancel.Any())
                throw new ArgumentException("No orders provided");

            var wrapper = new Dictionary<string, object>();
            var orderParameters = new Dictionary<string, object>[ordersToCancel!.Length];
            int i = 0;
            foreach (var order in ordersToCancel)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("resource", "ORDER");
                parameters.Add("operation", "DELETE");
                var orderParameter = new Dictionary<string, object>()
                {
                    {"id", order},
                };
                parameters.Add("payload", orderParameter);
                orderParameters[i] = parameters;
                i++;
            }
            wrapper.Add(string.Empty, orderParameters);

            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new BatchResultConverter<BittrexOrder>()
                }
            });

            return await SendRequestAsync<IEnumerable<CallResult<BittrexOrder>>>(GetUrl("batch"), HttpMethod.Post, ct, wrapper, signed: true, deserializer: serializer).ConfigureAwait(false);
        }
        #endregion

        #region common interface
#pragma warning disable 1066
        async Task<WebCallResult<IEnumerable<ICommonSymbol>>> IExchangeClient.GetSymbolsAsync()
        {
            var symbols = await GetSymbolsAsync().ConfigureAwait(false);
            return symbols.As<IEnumerable<ICommonSymbol>>(symbols.Data);
        }

        async Task<WebCallResult<ICommonOrderBook>> IExchangeClient.GetOrderBookAsync(string symbol)
        {
            var orderBookResult = await GetOrderBookAsync(symbol).ConfigureAwait(false);
            return orderBookResult.As<ICommonOrderBook>(orderBookResult.Data);
        }

        async Task<WebCallResult<ICommonTicker>> IExchangeClient.GetTickerAsync(string symbol)
        {
            var ticker = await GetSymbolSummaryAsync(symbol).ConfigureAwait(false);
            return ticker.As<ICommonTicker>(ticker.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonTicker>>> IExchangeClient.GetTickersAsync()
        {
            var tradesResult = await GetSymbolSummariesAsync().ConfigureAwait(false);
            return tradesResult.As<IEnumerable<ICommonTicker>>(tradesResult.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonRecentTrade>>> IExchangeClient.GetRecentTradesAsync(string symbol)
        {
            var tradesResult = await GetTradeHistoryAsync(symbol).ConfigureAwait(false);
            return tradesResult.As<IEnumerable<ICommonRecentTrade>>(tradesResult.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonKline>>> IExchangeClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            if (startTime.HasValue)
            {
                var interval = GetKlineIntervalFromTimespan(timespan);
                var klines = await GetHistoricalKlinesAsync(symbol, interval,
                    startTime.Value.Year,
                    interval == KlineInterval.OneDay ? null : (int?)startTime.Value.Month,
                    interval == KlineInterval.OneDay || interval == KlineInterval.OneHour ? null : (int?)startTime.Value.Day).ConfigureAwait(false);
                return klines.As<IEnumerable<ICommonKline>>(klines.Data);
            }
            else
            {
                var klines = await GetKlinesAsync(symbol, GetKlineIntervalFromTimespan(timespan)).ConfigureAwait(false);
                return klines.As<IEnumerable<ICommonKline>>(klines.Data);
            }
        }

        async Task<WebCallResult<ICommonOrderId>> IExchangeClient.PlaceOrderAsync(string symbol, IExchangeClient.OrderSide side, IExchangeClient.OrderType type, decimal quantity, decimal? price = null, string? accountId = null)
        {
            var result = await PlaceOrderAsync(symbol, GetOrderSide(side), GetOrderType(type), TimeInForce.GoodTillCanceled, quantity, price: price).ConfigureAwait(false);
            return result.As<ICommonOrderId>(result.Data);
        }

        async Task<WebCallResult<ICommonOrder>> IExchangeClient.GetOrderAsync(string orderId, string? symbol)
        {
            var result = await GetOrderAsync(orderId).ConfigureAwait(false);
            return result.As<ICommonOrder>(result.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonTrade>>> IExchangeClient.GetTradesAsync(string orderId, string? symbol = null)
        {
            var result = await GetUserTradesAsync(orderId).ConfigureAwait(false);
            return result.As<IEnumerable<ICommonTrade>>(result.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonOrder>>> IExchangeClient.GetOpenOrdersAsync(string? symbol)
        {
            var result = await GetOpenOrdersAsync().ConfigureAwait(false);
            return result.As<IEnumerable<ICommonOrder>>(result.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonOrder>>> IExchangeClient.GetClosedOrdersAsync(string? symbol)
        {
            var result = await GetClosedOrdersAsync(symbol).ConfigureAwait(false);
            return result.As<IEnumerable<ICommonOrder>>(result.Data);
        }

        async Task<WebCallResult<ICommonOrderId>> IExchangeClient.CancelOrderAsync(string orderId, string? symbol)
        {
            var result = await CancelOrderAsync(orderId).ConfigureAwait(false);
            return result.As<ICommonOrderId>(result.Data);
        }

        async Task<WebCallResult<IEnumerable<ICommonBalance>>> IExchangeClient.GetBalancesAsync(string? accountId = null)
        {
            var result = await GetBalancesAsync().ConfigureAwait(false);
            return result.As<IEnumerable<ICommonBalance>>(result.Data);
        }
#pragma warning restore 1066

        #endregion

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken data)
        {
            if (data["code"] == null)
                return new UnknownError("Unknown response from server", data);

            var info = (string)data["code"]!;
            if (data["detail"] != null)
                info += "; Details: " + (string)data["detail"]!;
            if (data["data"] != null)
                info += "; Data: " + data["data"];

            return new ServerError(info);
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, Dictionary<string, object> parameters, string contentType)
        {
            if (parameters.Any() && parameters.First().Key == string.Empty)
            {
                var stringData = JsonConvert.SerializeObject(parameters.First().Value);
                request.SetContent(stringData, contentType);
            }
            else
            {
                var stringData = JsonConvert.SerializeObject(parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));
                request.SetContent(stringData, contentType);
            }
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

        private static KlineInterval GetKlineIntervalFromTimespan(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.FromMinutes(1)) return KlineInterval.OneMinute;
            if (timeSpan == TimeSpan.FromMinutes(5)) return KlineInterval.FiveMinutes;
            if (timeSpan == TimeSpan.FromHours(1)) return KlineInterval.OneHour;
            if (timeSpan == TimeSpan.FromDays(1)) return KlineInterval.OneDay;

            throw new ArgumentException("Unsupported timespan for Bittrex Klines, check supported intervals using Bittrex.Net.Objects.KlineInterval");
        }

        private static OrderSide GetOrderSide(IExchangeClient.OrderSide side)
        {
            if (side == IExchangeClient.OrderSide.Sell) return OrderSide.Sell;
            if (side == IExchangeClient.OrderSide.Buy) return OrderSide.Buy;

            throw new ArgumentException("Unsupported order side for Bittrex order: " + side);
        }

        private static OrderType GetOrderType(IExchangeClient.OrderType type)
        {
            if (type == IExchangeClient.OrderType.Limit) return OrderType.Limit;
            if (type == IExchangeClient.OrderType.Market) return OrderType.Market;

            throw new ArgumentException("Unsupported order type for Bittrex order: " + type);
        }

        #endregion

        /// <inheritdoc />
        public string GetSymbolName(string baseAsset, string quoteAsset) => $"{baseAsset}-{quoteAsset}".ToUpperInvariant();
    }
}
