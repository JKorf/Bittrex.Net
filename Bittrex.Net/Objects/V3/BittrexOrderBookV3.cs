using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Order book
    /// </summary>
    public class BittrexOrderBookV3
    {
        /// <summary>
        /// The sequence number
        /// </summary>
        public long Sequence { get; set; }
        /// <summary>
        /// The bids in this book
        /// </summary>
        public IEnumerable<BittrexOrderBookEntryV3> Bid { get; set; } = new List<BittrexOrderBookEntryV3>();
        /// <summary>
        /// The asks in this book
        /// </summary>
        public IEnumerable<BittrexOrderBookEntryV3> Ask { get; set; } = new List<BittrexOrderBookEntryV3>();
    }

    /// <summary>
    /// Entry for the order book
    /// </summary>
    public class BittrexOrderBookEntryV3: ISymbolOrderBookEntry
    {
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the entry
        /// </summary>
        [JsonProperty("Rate")]
        public decimal Price { get; set; }
    }
}
