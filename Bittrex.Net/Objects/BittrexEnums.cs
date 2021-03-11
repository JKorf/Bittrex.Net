namespace Bittrex.Net.Objects
{
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
    public enum OrderType
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

    /// <summary>
    /// Type of klines
    /// </summary>
    public enum KlineType
    {
        /// <summary>
        /// Trade
        /// </summary>
        Trade,
        /// <summary>
        /// Mid point
        /// </summary>
        Midpoint
    }

    /// <summary>
    /// Condition order operand
    /// </summary>
    public enum BittrexConditionalOrderOperand
    {
        /// <summary>
        /// Price above
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Price below
        /// </summary>
        LesserThan
    }

}
