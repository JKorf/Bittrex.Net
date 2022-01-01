using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    public class BittrexOrderBook
    {
        /// <summary>
        /// The sequence number
        /// </summary>
        public long Sequence { get; set; }
        /// <summary>
        /// The bids in this book
        /// </summary>
        [JsonProperty("bid")]
        public IEnumerable<BittrexOrderBookEntry> Bids { get; set; } = Array.Empty<BittrexOrderBookEntry>();
        /// <summary>
        /// The asks in this book
        /// </summary>
        [JsonProperty("ask")]
        public IEnumerable<BittrexOrderBookEntry> Asks { get; set; } = Array.Empty<BittrexOrderBookEntry>();
    }

    /// <summary>
    /// Entry for the order book
    /// </summary>
    public class BittrexOrderBookEntry: ISymbolOrderBookEntry
    {
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the entry
        /// </summary>
        [JsonProperty("rate")]
        public decimal Price { get; set; }
    }
}
