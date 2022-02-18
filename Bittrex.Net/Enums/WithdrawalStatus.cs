namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Status of a withdrawal
    /// </summary>
    public enum WithdrawalStatus
    {
        /// <summary>
        /// Requested
        /// </summary>
        Requested,
        /// <summary>
        /// Authorized
        /// </summary>
        Authorized,
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Completed
        /// </summary>
        Completed,
        /// <summary>
        /// Failed; invalid address
        /// </summary>
        InvalidAddress,
        /// <summary>
        /// Canceled
        /// </summary>
        Canceled
    }
}
