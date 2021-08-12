namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Currency permission
    /// </summary>
    public class BittrexCurrencyPermission
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Allowed to view
        /// </summary>
        public bool View { get; set; }
        /// <summary>
        /// Allowed to buy
        /// </summary>
        public BittrexCurrencyDepositPermission Deposit { get; set; } = default!;
        /// <summary>
        /// Allowed to sell
        /// </summary>
        public BittrexCurrencyWithdrawPermission Withdraw { get; set; } = default!;
    }

    /// <summary>
    /// Deposit permission
    /// </summary>
    public class BittrexCurrencyDepositPermission
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
    }

    /// <summary>
    /// Withdraw permission
    /// </summary>
    public class BittrexCurrencyWithdrawPermission
    {
        /// <summary>
        /// Allowed to withdraw via blockchain
        /// </summary>
        public bool BlockChain { get; set; }
        /// <summary>
        /// Allowed to withdraw via wire transfer
        /// </summary>
        public bool WireTransfer { get; set; }
    }
}
