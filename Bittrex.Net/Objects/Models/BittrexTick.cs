
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Market tick
    /// </summary>
    public class BittrexTick
    {
        /// <summary>
        /// Symbol of the ticker
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The price of the last trade
        /// </summary>
        [JsonProperty("lastTradeRate")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// The highest bid price
        /// </summary>
        [JsonProperty("bidRate")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// The lowest ask price
        /// </summary>
        [JsonProperty("askRate")]
        public decimal BestAskPrice { get; set; }
    }
}
