using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// New batch order info
    /// </summary>
    public class BittrexNewBatchOrder
    {
        /// <summary>
        /// Symbol the order is for
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } 
        /// <summary>
        /// Order side
        /// </summary>
        [JsonProperty("direction"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonProperty("timeInForce"), JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonProperty("limit")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Ceiling
        /// </summary>
        [JsonProperty("ceiling")]
        public decimal? Ceiling { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Use awards
        /// </summary>
        [JsonProperty("useAwards")]
        public bool? UseAwards { get; set; }

        /// <summary>
        /// Create a new batch order
        /// </summary>
        /// <param name="symbol">Symbol of the order</param>
        /// <param name="side">Side of the order</param>
        /// <param name="type">Type of the order</param>
        /// <param name="timeInForce">Time in force</param>
        public BittrexNewBatchOrder(string symbol, OrderSide side, OrderType type, TimeInForce timeInForce)
        {
            Symbol = symbol;
            Side = side;
            Type = type;
            TimeInForce = timeInForce;
        }

    }
}
