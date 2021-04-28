using Bittrex.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects
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
        /// Order direction
        /// </summary>
        [JsonProperty("direction"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Direction { get; set; }
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
        /// Limit price
        /// </summary>
        [JsonProperty("limit")]
        public decimal? Limit { get; set; }
        /// <summary>
        /// Ceiling price
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
        /// <param name="direction">Direction of the order</param>
        /// <param name="type">Type of the order</param>
        /// <param name="timeInForce">Time in force</param>
        public BittrexNewBatchOrder(string symbol, OrderSide direction, OrderType type, TimeInForce timeInForce)
        {
            Symbol = symbol;
            Direction = direction;
            Type = type;
            TimeInForce = timeInForce;
        }

    }
}
