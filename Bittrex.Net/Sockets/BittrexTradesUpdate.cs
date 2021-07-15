using System;
using System.Collections.Generic;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Symbol trade update
    /// </summary>
    public class BittrexTradesUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Trades
        /// </summary>
        public IEnumerable<BittrexSymbolTrade> Deltas { get; set; } = Array.Empty<BittrexSymbolTrade>();
    }
}
