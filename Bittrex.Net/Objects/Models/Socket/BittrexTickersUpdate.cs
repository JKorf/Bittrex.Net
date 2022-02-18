using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Symbol tickers update
    /// </summary>
    public class BittrexTickersUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Ticker per symbol
        /// </summary>
        public IEnumerable<BittrexTick> Deltas { get; set; } = Array.Empty<BittrexTick>();
    }
}
