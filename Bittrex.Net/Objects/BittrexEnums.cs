namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Order book type
    /// </summary>
    public enum OrderBookType
    {
        /// <summary>
        /// Only show buy orders
        /// </summary>
        Buy,
        /// <summary>
        /// Only show sell orders
        /// </summary>
        Sell,
        /// <summary>
        /// Show all orders
        /// </summary>
        Both
    }

    /// <summary>
    /// Whether the order is partially or fully filled
    /// </summary>
    public enum FillType
    {
        Fill,
        PartialFill
    }

    public enum OrderType
    {
        Buy,
        Sell
    }

    public enum OrderTypeExtended
    {
        LimitBuy,
        LimitSell
    }

    public enum TickInterval
    {
        OneMinute,
        FiveMinutes,
        HalfHour,
        OneHour,
        OneDay
    }
}
