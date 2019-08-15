using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream market summary update
    /// </summary>
    public class BittrexStreamMarketSummaryUpdate
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
        public List<BittrexStreamMarketSummary> Deltas { get; set; }
    }

    /// <summary>
    /// Stream summary query response
    /// </summary>
    public class BittrexStreamMarketSummariesQuery
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
        public List<BittrexStreamMarketSummary> Deltas { get; set; }
    }
}
