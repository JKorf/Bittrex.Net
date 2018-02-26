using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order
    /// </summary>
    public class BittrexOrderHistoryOrder
    {
        /// <summary>
        /// Guid of the order
        /// </summary>
        public Guid OrderUuid { get; set; }
        /// <summary>
        /// Market the order is on
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// Timestamp when the order was opened
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideExtendedConverter))]
        public OrderSideExtended OrderType { get; set; }
        /// <summary>
        /// The limit of the order
        /// </summary>
        public decimal Limit { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The remaining quantity of the order
        /// </summary>
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// The commission paid for the order
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The price paid per unit
        /// </summary>
        public decimal PricePerUnit { get; set; }
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
        /// <summary>
        /// Whether the order was an ImmediateOrCancel order
        /// </summary>
        public bool ImmediateOrCancel { get; set; }
        /// <summary>
        /// Timestamp when the order was closed
        /// </summary>
        public DateTime? Closed { get; set; }
        /// <summary>
        /// Whether the order is being canceled
        /// </summary>
        public bool CancelInitiated { get; set; }
    }
}
