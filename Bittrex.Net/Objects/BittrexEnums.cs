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

    public enum OrderSide
    {
        Buy,
        Sell
    }

    public enum OrderType
    {
        Limit,
        Market
    }

    public enum OrderSideExtended
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

    public enum TimeInEffect
    {
        GoodTillCancelled,
        ImmediateOrCancel
    }

    public enum ConditionType
    {
        None,
        GreaterThan,
        LessThan,
        StopLossFixed,
        StopLossPercentage
    }

    public enum OrderUpdateType
    {
        Open,
        PartialFill,
        Fill,
        Cancel
    }
}
