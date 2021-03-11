using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects;

namespace Bittrex.Net.Sockets
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
        public IEnumerable<BittrexSymbolSummary> Deltas { get; set; } = new List<BittrexSymbolSummary>();
    }
}
