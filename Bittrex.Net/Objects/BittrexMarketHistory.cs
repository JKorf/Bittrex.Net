using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order executed on a market
    /// </summary>
    public class BittrexMarketHistory
    {
        /// <summary>
        /// The order id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Timestamp of the order
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Total price of the order
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Whether the order was fully filled
        /// </summary>
        [JsonConverter(typeof(FillTypeConverter))]
        public FillType FillType { get; set; }
        /// <summary>
        /// The type of the order
        /// </summary>
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
    }
}
