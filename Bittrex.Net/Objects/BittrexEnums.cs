namespace Bittrex.Net.Objects
{
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

    public enum TimeInForce
    {
        GoodTillCancelled,
        ImmediateOrCancel,
        FillOrKill,
        PostOnlyGoodTillCancelled
    }

    public enum OrderTypeV3
    {
        Limit,
        Market,
        CeilingLimit,
        CeilingMarket
    }

    public enum DepositAddressStatus
    {
        Requested,
        Provisioned
    }

    public enum DepositStatus
    {
        Pending,
        Completed,
        Orphaned,
        Invalidated
    }

    public enum SymbolStatus
    {
        Online,
        Offline
    }

    public enum OrderStatus
    {
        Open,
        Closed
    }

    public enum WithdrawalStatus
    {
        Requested,
        Authorized,
        Pending,
        Completed,
        InvalidAddress,
        Cancelled
    }

    public enum CandleInterval
    {
        OneMinute,
        FiveMinutes,
        OneHour,
        OneDay
    }

}
