using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models.Socket
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
        public IEnumerable<BittrexTrade> Deltas { get; set; } = Array.Empty<BittrexTrade>();
    }
}
