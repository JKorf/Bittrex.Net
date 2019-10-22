using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream symbol summary update
    /// </summary>
    internal class BittrexStreamMarketSummaryUpdate
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// The current market summaries
        /// </summary>
        [JsonProperty("D")]
        public IEnumerable<BittrexStreamSymbolSummary> Deltas { get; set; } = new List<BittrexStreamSymbolSummary>();
    }

    /// <summary>
    /// Stream symbol summary response
    /// </summary>
    internal class BittrexStreamMarketSummariesQuery
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// The current market summaries
        /// </summary>
        [JsonProperty("s")]
        public IEnumerable<BittrexStreamSymbolSummary> Deltas { get; set; } = new List<BittrexStreamSymbolSummary>();
    }
}
