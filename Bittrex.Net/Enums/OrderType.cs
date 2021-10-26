namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Type of order
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit order; place at specific price
        /// </summary>
        Limit,
        /// <summary>
        /// Symbol order; execute order at best price on placing
        /// </summary>
        Market
    }
}
