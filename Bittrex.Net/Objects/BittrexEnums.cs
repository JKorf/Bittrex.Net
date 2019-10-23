namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Whether the order is partially or fully filled
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// Filled
        /// </summary>
        Fill,
        /// <summary>
        /// Partially filled
        /// </summary>
        PartialFill
    }

    /// <summary>
    /// Side of an order
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        Sell
    }

    /// <summary>
    /// The type of order
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit order; place an order for a specific price
        /// </summary>
        Limit,
        /// <summary>
        /// Symbol order; execute order at the best price available when placed
        /// </summary>
        Market
    }

    /// <summary>
    /// Order type and side
    /// </summary>
    public enum OrderSideExtended
    {
        /// <summary>
        /// Buy limit order
        /// </summary>
        LimitBuy,
        /// <summary>
        /// Sell limit order
        /// </summary>
        LimitSell,
        /// <summary>
        /// Buy market order
        /// </summary>
        MarketBuy,
        /// <summary>
        /// Sell market order
        /// </summary>
        MarketSell
    }

    /// <summary>
    /// Interval for klines
    /// </summary>
    public enum TickInterval
    {
        /// <summary>
        /// 1m
        /// </summary>
        OneMinute,
        /// <summary>
        /// 5m
        /// </summary>
        FiveMinutes,
        /// <summary>
        /// 30m
        /// </summary>
        HalfHour,
        /// <summary>
        /// 1h
        /// </summary>
        OneHour,
        /// <summary>
        /// 1d
        /// </summary>
        OneDay
    }

    /// <summary>
    /// The time an order is active
    /// </summary>
    public enum TimeInEffect
    {
        /// <summary>
        /// Order will be active until cancelled
        /// </summary>
        GoodTillCancelled,
        /// <summary>
        /// Order has to be at least partially filled or it will be canceled on placing
        /// </summary>
        ImmediateOrCancel
    }

    /// <summary>
    /// Type of condition
    /// </summary>
    public enum ConditionType
    {
        /// <summary>
        /// No condition
        /// </summary>
        None,
        /// <summary>
        /// Greater than
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Lesser than
        /// </summary>
        LessThan,
        /// <summary>
        /// Stop loss fixed
        /// </summary>
        StopLossFixed,
        /// <summary>
        /// Stop loss percentage
        /// </summary>
        StopLossPercentage
    }

    /// <summary>
    /// Update type for an order
    /// </summary>
    public enum OrderUpdateType
    {
        /// <summary>
        /// Open
        /// </summary>
        Open,
        /// <summary>
        /// Partially filled
        /// </summary>
        PartialFill,
        /// <summary>
        /// Fully filled
        /// </summary>
        Fill,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancel
    }

    /// <summary>
    /// Time an order is active
    /// </summary>
    public enum TimeInForce
    {
        /// <summary>
        /// Active until cancelled
        /// </summary>
        GoodTillCancelled,
        /// <summary>
        /// Has to be at least partially filled upon placing or cancelled
        /// </summary>
        ImmediateOrCancel,
        /// <summary>
        /// Has to be fully filled upon placing or cancelled
        /// </summary>
        FillOrKill,
        /// <summary>
        /// Post only good till cancel
        /// </summary>
        PostOnlyGoodTillCancelled,
        /// <summary>
        /// Post only good till specific date
        /// </summary>
        PostOnlyGoodTillDate
    }

    /// <summary>
    /// Type of order
    /// </summary>
    public enum OrderTypeV3
    {
        /// <summary>
        /// Limit order; place at specific price
        /// </summary>
        Limit,
        /// <summary>
        /// Symbol order; execute order at best price on placing
        /// </summary>
        Market,
        /// <summary>
        /// Ceiling limit order; a limit order filling for a specific amount of base currency
        /// </summary>
        CeilingLimit,
        /// <summary>
        /// Ceiling market order; a market order filling for a specific amount of base currency
        /// </summary>
        CeilingMarket
    }

    /// <summary>
    /// Deposit address state
    /// </summary>
    public enum DepositAddressStatus
    {
        /// <summary>
        /// Requested
        /// </summary>
        Requested,
        /// <summary>
        /// Provisioned
        /// </summary>
        Provisioned
    }

    /// <summary>
    /// Status of a deposit
    /// </summary>
    public enum DepositStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Completed
        /// </summary>
        Completed,
        /// <summary>
        /// Orphaned
        /// </summary>
        Orphaned,
        /// <summary>
        /// Invalidated
        /// </summary>
        Invalidated
    }

    /// <summary>
    /// Status of a symbol
    /// </summary>
    public enum SymbolStatus
    {
        /// <summary>
        /// Online
        /// </summary>
        Online,
        /// <summary>
        /// Offline, not tradable
        /// </summary>
        Offline
    }

    /// <summary>
    /// Status of an order
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order is open/active
        /// </summary>
        Open,
        /// <summary>
        /// Order is closed
        /// </summary>
        Closed
    }

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
        /// Cancelled
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// Interval of klines
    /// </summary>
    public enum KlineInterval
    {
        /// <summary>
        /// 1m
        /// </summary>
        OneMinute,
        /// <summary>
        /// 5m
        /// </summary>
        FiveMinutes,
        /// <summary>
        /// 1h
        /// </summary>
        OneHour,
        /// <summary>
        /// 1d
        /// </summary>
        OneDay
    }

}
