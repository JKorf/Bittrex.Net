namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Time an order is active
    /// </summary>
    public enum TimeInForce
    {
        /// <summary>
        /// Active until canceled
        /// </summary>
        GoodTillCanceled,
        /// <summary>
        /// Has to be at least partially filled upon placing or canceled
        /// </summary>
        ImmediateOrCancel,
        /// <summary>
        /// Has to be fully filled upon placing or canceled
        /// </summary>
        FillOrKill,
        /// <summary>
        /// Post only good till cancel
        /// </summary>
        PostOnlyGoodTillCanceled,
        /// <summary>
        /// Post only good till specific date
        /// </summary>
        PostOnlyGoodTillDate
    }
}
