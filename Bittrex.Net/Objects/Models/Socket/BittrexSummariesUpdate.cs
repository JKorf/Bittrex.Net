using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Symbol summaries update
    /// </summary>
    public class BittrexSummariesUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Summary per symbol
        /// </summary>
        public IEnumerable<BittrexSymbolSummary> Deltas { get; set; } = Array.Empty<BittrexSymbolSummary>();
    }
}
