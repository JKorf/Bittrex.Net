using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Interfaces.Clients.SpotApi;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BittrexRestClientSpotApiAccount : IBittrexRestClientSpotApiAccount
    {
        private readonly BittrexRestClientSpotApi _baseClient;

        internal BittrexRestClientSpotApiAccount(BittrexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexAssetPermission>>(_baseClient.GetUrl("account/permissions/currencies/" + asset), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexAssetPermission>>(_baseClient.GetUrl("account/permissions/currencies"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateNotNull(nameof(symbol));
            symbol.ValidateBittrexSymbol();

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexSymbolPermission>>(_baseClient.GetUrl("account/permissions/markets/" + symbol), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexSymbolPermission>>(_baseClient.GetUrl("account/permissions/markets"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<BittrexAccount>(_baseClient.GetUrl("account"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTradingFee>>> GetTradingFeesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexTradingFee>>(_baseClient.GetUrl("account/fees/trading"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexFiatFee>>> GetFiatTransactionFeesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexFiatFee>>(_baseClient.GetUrl("account/fees/fiat"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<BittrexAccountVolume>(_baseClient.GetUrl("account/volume"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        #region balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexBalance>>(_baseClient.GetUrl("balances"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await _baseClient.SendRequestAsync<BittrexBalance>(_baseClient.GetUrl($"balances/{asset}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion

        #region addresses

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDepositAddress>>> GetDepositAddressesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexDepositAddress>>(_baseClient.GetUrl("addresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await _baseClient.SendRequestAsync<BittrexDepositAddress>(_baseClient.GetUrl($"addresses/{asset}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDepositAddress>> RequestDepositAddressAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>()
            {
                { "currencySymbol", asset }
            };

            return await _baseClient.SendRequestAsync<BittrexDepositAddress>(_baseClient.GetUrl("addresses"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region deposits

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetOpenDepositsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexDeposit>>(_baseClient.GetUrl("deposits/open"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexDeposit>>(_baseClient.GetUrl("deposits/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexDeposit>>(_baseClient.GetUrl($"deposits/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexDeposit>> GetDepositAsync(string depositId, CancellationToken ct = default)
        {
            depositId.ValidateNotNull(nameof(depositId));
            return await _baseClient.SendRequestAsync<BittrexDeposit>(_baseClient.GetUrl($"deposits/{depositId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        #endregion

        #region withdrawals

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetOpenWithdrawalsAsync(string? asset = null, WithdrawalStatus? status = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("currencySymbol", asset);
            parameters.AddOptionalParameter("status", status);

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexWithdrawal>>(_baseClient.GetUrl("withdrawals/open"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
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

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexWithdrawal>>(_baseClient.GetUrl("withdrawals/closed"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default)
        {
            transactionId.ValidateNotNull(nameof(transactionId));
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexWithdrawal>>(_baseClient.GetUrl($"withdrawals/ByTxId/{transactionId}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexWithdrawal>> GetWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await _baseClient.SendRequestAsync<BittrexWithdrawal>(_baseClient.GetUrl($"withdrawals/{id}"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexWithdrawal>> CancelWithdrawalAsync(string id, CancellationToken ct = default)
        {
            id.ValidateNotNull(nameof(id));
            return await _baseClient.SendRequestAsync<BittrexWithdrawal>(_baseClient.GetUrl($"withdrawals/{id}"), HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
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

            return await _baseClient.SendRequestAsync<BittrexWithdrawal>(_baseClient.GetUrl("withdrawals"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexWhitelistAddress>>(_baseClient.GetUrl("withdrawals/whitelistAddresses"), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }
        #endregion
    }
}
