using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamMarketSummariesLite
    {
        [JsonProperty("D")]
        public List<BittrexStreamMarketSummaryLite> Deltas { get; set; }
    }

    public class BittrexStreamMarketSummaryLite
    {
        /// <summary>
        /// Name of the market
        /// </summary>
        [JsonProperty("M")]
        public string MarketName { get; set; }
        /// <summary>
        /// Price of last executed trade
        /// </summary>
        [JsonProperty("l")]
        public decimal Last { get; set; }
        /// <summary>
        /// The base volume
        /// </summary>
        [JsonProperty("m")]
        public decimal BaseVolume { get; set; }
    }
}
