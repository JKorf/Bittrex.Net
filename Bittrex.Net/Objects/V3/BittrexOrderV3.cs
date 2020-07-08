using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Data on an unplaced order
    /// </summary>
    public class BittrexUnplacedOrder
    {
        /// <summary>
        /// The symbol of the order
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The direction of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Direction { get; set; }
        /// <summary>
        /// The type of order
        /// </summary>
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderTypeV3 Type { get; set; }
        /// <summary>
        /// The quantity of the order
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// The limit of the order
        /// </summary>
        public decimal? Limit { get; set; }
        /// <summary>
        /// The ceiling of the order
        /// </summary>
        public decimal? Ceiling { get; set; }
        /// <summary>
        /// The time in force of the order
        /// </summary>
        [JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce TimeInForce { get; set; }

        /// <summary>
        /// Id to track the order by
        /// </summary>
        public string ClientOrderId { get; set; } = "";
    }

    /// <summary>
    /// Bittrex order info
    /// </summary>
    public class BittrexOrderV3: BittrexUnplacedOrder
    {
        /// <summary>
        /// The id of the order
        /// </summary>
        public string Id { get; set; } = "";
        
        /// <summary>
        /// The quantity that's been filled
        /// </summary>
        public decimal FillQuantity { get; set; }
        /// <summary>
        /// The commission paid for this order
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// The proceeds of this order
        /// </summary>
        public decimal Proceeds { get; set; }
        /// <summary>
        /// The status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// When the order was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// When the order was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// When the order was closed
        /// </summary>
        public DateTime? ClosedAt { get; set; }

        /// <summary>
        /// Conditional order to cancel if this order executes
        /// </summary>
        public BittrexLinkedOrder? OrderToCancel { get; set; }
    }
}
