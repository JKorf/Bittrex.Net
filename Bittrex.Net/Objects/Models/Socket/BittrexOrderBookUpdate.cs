using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models.Socket
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
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Depth
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// Bid changes
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> BidDeltas { get; set; } = Array.Empty<BittrexOrderBookEntry>();
        /// <summary>
        /// Ask changes
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> AskDeltas { get; set; } = Array.Empty<BittrexOrderBookEntry>();
    }
}
