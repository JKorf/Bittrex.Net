using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order
    /// </summary>
    public class BittrexAccountOrder
    {
        /// <summary>
        /// Account id associated with the order
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// The order Guid
        /// </summary>
        public Guid OrderUuid { get; set; }
        /// <summary>
        /// The market the order is on
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// The order type
        /// </summary>
        [JsonConverter(typeof(OrderTypeExtendedConverter))]
        public OrderTypeExtended Type { get; set; }
        /// <summary>
        /// The quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The remaining quantity of the order
        /// </summary>
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// The order limit
        /// </summary>
        public decimal Limit { get; set; }
        /// <summary>
        /// The reserved currency for this order
        /// </summary>
        public decimal Reserved { get; set; }
        /// <summary>
        /// The remaining reserved currency for this order
        /// </summary>
        public decimal ReservedRemaining { get; set; }
        /// <summary>
        /// The commission reserverd for this order
        /// </summary>
        public decimal CommissionReserved { get; set; }
        /// <summary>
        /// The remaining commission reserved for this order
        /// </summary>
        public decimal CommissionReservedRemaining { get; set; }
        /// <summary>
        /// The amount of commission paid for this order
        /// </summary>
        public decimal CommissionPaid { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The price per unit
        /// </summary>
        public decimal? PricePerUnit { get; set; }
        /// <summary>
        /// Timestamp when order was opened
        /// </summary>
        public DateTime Opened { get; set; }
        /// <summary>
        /// Timestamp when order was closed
        /// </summary>
        public DateTime? Closed { get; set; }
        /// <summary>
        /// Whether the order is still open
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Sentinel { get; set; }
        /// <summary>
        /// Whether a cancel has begun processing
        /// </summary>
        public bool CancelInitiated { get; set; }
        /// <summary>
        /// Whether it is a ImmediateOrCancel order
        /// </summary>
        public bool ImmediateOrCancel { get; set; }
        /// <summary>
        /// Whether the order is conditional
        /// </summary>
        public bool IsConditional { get; set; }
        /// <summary>
        /// The condition of the order
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// The condition target of the order
        /// </summary>
        public string ConditionTarget { get; set; }
    }
}
