using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamMarketSummaryUpdate
    {
        /// <summary>
        /// Nounce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// The current market summaries
        /// </summary>
        [JsonProperty("D")]
        public List<BittrexStreamMarketSummary> Deltas { get; set; }
    }

    public class BittrexStreamMarketSummariesQuery
    {
        /// <summary>
        /// Nounce
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
