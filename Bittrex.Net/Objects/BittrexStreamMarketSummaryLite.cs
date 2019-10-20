using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    internal class BittrexStreamMarketSummariesLite
    {
        [JsonProperty("D")]
        public IEnumerable<BittrexStreamMarketSummaryLite> Deltas { get; set; } = new List<BittrexStreamMarketSummaryLite>();
    }

    /// <summary>
    /// Stream lite market symmary
    /// </summary>
    public class BittrexStreamMarketSummaryLite
    {
        /// <summary>
        /// Name of the market
        /// </summary>
        [JsonProperty("M")]
        public string MarketName { get; set; } = "";
        /// <summary>
        /// Price of last executed trade
        /// </summary>
        [JsonProperty("l")]
        public decimal? Last { get; set; }
        /// <summary>
        /// The base volume
        /// </summary>
        [JsonProperty("m")]
        public decimal? BaseVolume { get; set; }
    }
}
