using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order
    /// </summary>
    public class BittrexOrder
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid? Uuid { get; set; }
        /// <summary>
        /// Guid of the order
        /// </summary>
        public Guid OrderUuid { get; set; }
        /// <summary>
        /// Market the order is on
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// The type of the order
        /// </summary>
        [JsonConverter(typeof(OrderTypeExtendedConverter))]
        public OrderTypeExtended OrderType { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// The remaining quantity of the order
        /// </summary>
        public double QuantityRemaining { get; set; }
        /// <summary>
        /// The limit of the order
        /// </summary>
        public double Limit { get; set; }
        /// <summary>
        /// The commission paid for the order
        /// </summary>
        public double CommissionPaid { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// The price paid per unit
        /// </summary>
        public double? PricePerUnit { get; set; }
        /// <summary>
        /// Timestamp when the order was opened
        /// </summary>
        public DateTime Opened { get; set; }
        /// <summary>
        /// Timestamp when the order was closed
        /// </summary>
        public DateTime? Closed { get; set; }
        /// <summary>
        /// Whether the order is being canceled
        /// </summary>
        public bool CancelInitiated { get; set; }
        /// <summary>
        /// Whether the order was an ImmediateOrCancel order
        /// </summary>
        public bool ImmediateOrCancel { get; set; }
        /// <summary>
        /// Whether the order is conditional
        /// </summary>
        public bool IsConditional { get; set; }
        /// <summary>
        /// The order condition
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// The order condition target
        /// </summary>
        public string ConditionTarget { get; set; }
    }
}
