using Bittrex.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order executed on a market
    /// </summary>
    public class BittrexStreamMarketHistory
    {
        /// <summary>
        /// The order id
        /// </summary>
        [JsonProperty("I")]
        public long Id { get; set; }
        /// <summary>
        /// Timestamp of the order
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        [JsonProperty("T")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        [JsonProperty("Q")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the order
        /// </summary>
        [JsonProperty("P")]
        public decimal Price { get; set; }
        /// <summary>
        /// Total price of the order
        /// </summary>
        [JsonProperty("t")]
        public decimal Total { get; set; }
        /// <summary>
        /// Whether the order was fully filled
        /// </summary>
        [JsonConverter(typeof(FillTypeConverter))]
        [JsonProperty("F")]
        public FillType FillType { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        [JsonProperty("OT")]
        public OrderSide OrderType { get; set; }
    }
}
