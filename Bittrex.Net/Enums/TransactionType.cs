using CryptoExchange.Net.Attributes;

namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Transaction type
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Deposit
        /// </summary>
        [Map("DEPOSIT")]
        Deposit,
        /// <summary>
        /// Withdrawal
        /// </summary>
        [Map("WITHDRAWAL")]
        Withdrawal
    }
}
