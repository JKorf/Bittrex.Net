using Bittrex.Net.Enums;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces.Clients.Rest
{
    public interface IBittrexClientAccount
    {
        /// <summary>
        /// Get permissions for a specific asset
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get permissions for all assets
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BittrexAssetPermission>>> GetAssetPermissionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get permissions for a specific symbol
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get permissions for all symbols
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<BittrexSymbolPermission>>> GetSymbolPermissionsAsync(CancellationToken ct = default);


        /// <summary>
        /// Get account info
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account info</returns>
        Task<WebCallResult<BittrexAccount>> GetAccountAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account trading fees
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account trading fee</returns>
        Task<WebCallResult<IEnumerable<BittrexTradingFee>>> GetTradingFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account volume
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account volume</returns>
        Task<WebCallResult<BittrexAccountVolume>> GetAccountVolumeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets current balances. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of balances</returns>
        Task<WebCallResult<IEnumerable<BittrexBalance>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets current balance for an asset
        /// </summary>
        /// <param name="asset">The name of the asset to get balance for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Balance for asset</returns>
        Task<WebCallResult<BittrexBalance>> GetBalanceAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposit addresses
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposit addresses</returns>
        Task<WebCallResult<IEnumerable<BittrexDepositAddress>>> GetDepositAddressesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets deposit addresses for an asset
        /// </summary>
        /// <param name="asset">The name of the asset to get the deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit addresses</returns>
        Task<WebCallResult<BittrexDepositAddress>> GetDepositAddressAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Request a deposit address for an asset
        /// </summary>
        /// <param name="asset">The name of the asset to get request a deposit address for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The deposit address</returns>
        Task<WebCallResult<BittrexDepositAddress>> RequestDepositAddressAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Gets list of open deposits. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// </summary>
        /// <param name="asset">Filter the list by asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetOpenDepositsAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of closed deposits
        /// </summary>
        /// <param name="asset">Filter the list by asset</param>
        /// <param name="status">Filter the list by status of the deposit</param>
        /// <param name="startTime">Filter the list by time</param>
        /// <param name="endTime">Filter the list by time</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last deposit id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first deposit id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetClosedDepositsAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets list of deposits for a transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<BittrexDeposit>>> GetDepositsByTransactionIdAsync(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets a deposit by id
        /// </summary>
        /// <param name="depositId">The id of the deposit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit info</returns>
        Task<WebCallResult<BittrexDeposit>> GetDepositAsync(string depositId, CancellationToken ct = default);


        /// <summary>
        /// Gets a list of open withdrawals
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="status">Filter by status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetOpenWithdrawalsAsync(string? asset = null, WithdrawalStatus? status = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of closed withdrawals
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startTime">Filter the list by time</param>
        /// <param name="endTime">Filter the list by time</param>
        /// <param name="pageSize">The max amount of results to return</param>
        /// <param name="nextPageToken">The id of the object after which to return results. Typically the last withdrawal id of the previous page</param>
        /// <param name="previousPageToken">The id of the object before which to return results. Typically the first withdrawal id of the next page</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of closed withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetClosedWithdrawalsAsync(string? asset = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? pageSize = null, string? nextPageToken = null, string? previousPageToken = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of withdrawals by transaction id
        /// </summary>
        /// <param name="transactionId">The id of the transaction</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawals</returns>
        Task<WebCallResult<IEnumerable<BittrexWithdrawal>>> GetWithdrawalsByTransactionIdAsync(string transactionId, CancellationToken ct = default);

        /// <summary>
        /// Gets withdrawal by id
        /// </summary>
        /// <param name="id">The id of the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawal>> GetWithdrawalAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Cancels a withdrawal
        /// </summary>
        /// <param name="id">The id of the withdrawal to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal info</returns>
        Task<WebCallResult<BittrexWithdrawal>> CancelWithdrawalAsync(string id, CancellationToken ct = default);

        /// <summary>
        /// Withdraw from Bittrex
        /// </summary>
        /// <param name="asset">The asset to withdraw</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="address">The address to withdraw to</param>
        /// <param name="addressTag">A tag for the address</param>
        /// <param name="clientWithdrawId">Client id to identify the withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Info about the withdrawal</returns>
        Task<WebCallResult<BittrexWithdrawal>> WithdrawAsync(string asset, decimal quantity, string address, string? addressTag = null, string? clientWithdrawId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of whitelisted address for withdrawals
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List withdrawal address</returns>
        Task<WebCallResult<IEnumerable<BittrexWhitelistAddress>>> GetWithdrawalWhitelistAddressesAsync(CancellationToken ct = default);


    }
}
