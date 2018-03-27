using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamDeltas
    {
        /// <summary>
        /// Nounce
        /// </summary>
        public long Nounce { get; set; }

        /// <summary>
        /// The current market summaries
        /// </summary>
        public List<BittrexMarketSummary> Deltas { get; set; }
    }
}
