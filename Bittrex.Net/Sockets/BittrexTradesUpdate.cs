using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;
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
        public string Symbol { get; set; } = "";

        /// <summary>
        /// Trades
        /// </summary>
        public IEnumerable<BittrexSymbolTrade> Deltas { get; set; } = new List<BittrexSymbolTrade>();
    }
}
