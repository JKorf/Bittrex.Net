using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Order book update data
    /// </summary>
    public class BittrexOrderBookUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Symbol of the update
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Depth
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// Bid changes
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> BidDeltas { get; set; } = new List<BittrexOrderBookEntry>();
        /// <summary>
        /// Ask changes
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> AskDeltas { get; set; } = new List<BittrexOrderBookEntry>();
    }
}
