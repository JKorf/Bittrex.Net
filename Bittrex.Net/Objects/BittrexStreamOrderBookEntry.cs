using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamOrderBookEntry
    {
        /// <summary>
        /// Total quantity of order at this price
        /// </summary>
        [JsonProperty("Q")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the orders
        /// </summary>
        [JsonProperty("R")]
        public decimal Rate { get; set; }
    }

    public class BittrexStreamOrderBookUpdateEntry: BittrexStreamOrderBookEntry
    {
        /// <summary>
        /// how to handle data (used by stream)
        /// </summary>
        [JsonProperty("TY")]
        public OrderBookEntryType Type { get; set; }
    }
}
