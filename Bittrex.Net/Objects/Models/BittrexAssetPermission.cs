namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Asset permission
    /// </summary>
    public class BittrexAssetPermission
    {
        /// <summary>
        /// Symbol of the asset
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Allowed to view
        /// </summary>
        public bool View { get; set; }
        /// <summary>
        /// Allowed to buy
        /// </summary>
        public BittrexAssetDepositPermission Deposit { get; set; } = default!;
        /// <summary>
        /// Allowed to sell
        /// </summary>
        public BittrexAssetWithdrawPermission Withdraw { get; set; } = default!;
    }

    /// <summary>
    /// Deposit permission
    /// </summary>
    public class BittrexAssetDepositPermission
    {
        /// <summary>
        /// Allow to deposit via blockchain
        /// </summary>
        public bool BlockChain { get; set; }
        /// <summary>
        /// Allowed to deposit via credit card
        /// </summary>
        public bool CreditCard { get; set; }
        /// <summary>
        /// Allowed to deposit via wire transfer
        /// </summary>
        public bool WireTransfer { get; set; }
        /// <summary>
        /// Allowed  to deposit via ACH
        /// </summary>
        public bool Ach { get; set; }
    }

    /// <summary>
    /// Withdraw permission
    /// </summary>
    public class BittrexAssetWithdrawPermission
    {
        /// <summary>
        /// Allowed to withdraw via blockchain
        /// </summary>
        public bool BlockChain { get; set; }
        /// <summary>
        /// Allowed to withdraw via wire transfer
        /// </summary>
        public bool WireTransfer { get; set; }
        /// <summary>
        /// Allowed  to deposit via ACH
        /// </summary>
        public bool Ach { get; set; }
    }
}
