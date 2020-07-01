using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Order book
    /// </summary>
    public class BittrexOrderBookV3
    {
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
    public class BittrexOrderBookEntryV3
    {
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the entry
        /// </summary>
        public decimal Rate { get; set; }
    }

    /// <summary>
    /// Trade entry
    /// </summary>
    public class BittrexSymbolTradeV3 : BittrexOrderBookEntryV3
    {
        /// <summary>
        /// The timestamp of the trade execution
        /// </summary>
        public DateTime ExecutedAt { get; set; }
    }
}
