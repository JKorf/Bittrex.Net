using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Info on a trade
    /// </summary>
    public class BittrexTrade
    {
        /// <summary>
        /// Unique id of the trade
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Side of the taker
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide TakerSide { get; set; }
        /// <summary>
        /// The quantity of the trade
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the trade
        /// </summary>
        [JsonProperty("rate")]
        public decimal Price { get; set; }
        /// <summary>
        /// The timestamp of the trade execution
        /// </summary>
        [JsonProperty("executedAt")]
        public DateTime Timestamp { get; set; }
    }
}
