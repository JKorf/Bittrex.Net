using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream market summary update
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
        public IEnumerable<BittrexStreamMarketSummary> Deltas { get; set; } = new List<BittrexStreamMarketSummary>();
    }

    /// <summary>
    /// Stream summary query response
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
        public IEnumerable<BittrexStreamMarketSummary> Deltas { get; set; } = new List<BittrexStreamMarketSummary>();
    }
}
