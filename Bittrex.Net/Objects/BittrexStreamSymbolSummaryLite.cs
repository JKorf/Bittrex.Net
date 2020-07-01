using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    internal class BittrexStreamMarketSummariesLite
    {
        [JsonProperty("D")]
        public IEnumerable<BittrexStreamSymbolSummaryLite> Deltas { get; set; } = new List<BittrexStreamSymbolSummaryLite>();
    }

    /// <summary>
    /// Stream lite symbol summary
    /// </summary>
    public class BittrexStreamSymbolSummaryLite
    {
        /// <summary>
        /// Name of the symbol
        /// </summary>
        [JsonProperty("M")]
        public string Symbol { get; set; } = "";
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
