using Bittrex.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Info on a trade
    /// </summary>
    public class BittrexSymbolTrade
    {
        /// <summary>
        /// Unique id of the trade
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// Side of the taker
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide TakerSide { get; set; }
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the entry
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// The timestamp of the trade execution
        /// </summary>
        public DateTime ExecutedAt { get; set; }
    }
}
