using Bittrex.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamOrderData
    {
        /// <summary>
        /// Account id associated with the order
        /// </summary>
        [JsonProperty("w")]
        public Guid AccountId { get; set; }
        [JsonProperty("N")]
        public long Nonce { get; set; }
        /// <summary>
        /// The trigger for this update
        /// </summary>
        [JsonProperty("TY"), JsonConverter(typeof(OrderUpdateTypeConverter))]
        public OrderUpdateType Type { get; set; }
        /// <summary>
        /// Order information
        /// </summary>
        [JsonProperty("o")]
        public BittrexStreamOrder Order { get; set; }
    }

    public class BittrexStreamOrder
    {
        /// <summary>
        /// The unique identifier
        /// </summary>
        [JsonProperty("U")]
        public Guid Uuid { get; set; }
        [JsonProperty("I")]
        public long Id { get; set; }
        /// <summary>
        /// The order id
        /// </summary>
        [JsonProperty("OU")]
        public Guid OrderId { get; set; }
        /// <summary>
        /// The market this order is for
        /// </summary>
        [JsonProperty("E")]
        public string Market { get; set; }
        /// <summary>
        /// The order type
        /// </summary>
        [JsonProperty("OT"), JsonConverter(typeof(OrderSideExtendedConverter))]
        public OrderSideExtended OrderType { get; set; }
        /// <summary>
        /// The quantity of the order
        /// </summary>
        [JsonProperty("Q")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The remaining quantity of the order
        /// </summary>
        [JsonProperty("q")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// The order limit
        /// </summary>
        [JsonProperty("X")]
        public decimal Limit { get; set; }
        /// <summary>
        /// The amount of commission paid for this order
        /// </summary>
        [JsonProperty("n")]
        public decimal CommissionPaid { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        [JsonProperty("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// The price per unit
        /// </summary>
        [JsonProperty("PU")]
        public decimal? PricePerUnit { get; set; }
        /// <summary>
        /// Timestamp when order was opened
        /// </summary>
        [JsonProperty("Y"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Opened { get; set; }
        /// <summary>
        /// Timestamp when order was closed
        /// </summary>
        [JsonProperty("C"), JsonConverter(typeof(TimestampConverter))]
        public DateTime? Closed { get; set; }
        /// <summary>
        /// Whether the order is still open
        /// </summary>
        [JsonProperty("i")]
        public bool? IsOpen { get; set; }
        /// <summary>
        /// Whether a cancel is initiated for this order
        /// </summary>
        [JsonProperty("CI")]
        public bool CancelInitiated { get; set; }
        /// <summary>
        /// Whether it is a ImmediateOrCancel order
        /// </summary>
        [JsonProperty("K")]
        public bool ImmediateOrCancel { get; set; }
        /// <summary>
        /// Whether the order is conditional
        /// </summary>
        [JsonProperty("k")]
        public bool IsConditional { get; set; }
        /// <summary>
        /// The condition of the order
        /// </summary>
        [JsonProperty("J"), JsonConverter(typeof(ConditionTypeConverter))]
        public ConditionType Condition { get; set; }
        /// <summary>
        /// The condition target of the order
        /// </summary>
        [JsonProperty("j")]
        public decimal? ConditionTarget { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonProperty("u"), JsonConverter(typeof(TimestampConverter))]
        public DateTime? Updated { get; set; }
    }
}
