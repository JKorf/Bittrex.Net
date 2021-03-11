using System.Collections.Generic;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Order book
    /// </summary>
    public class BittrexOrderBook: ICommonOrderBook
    {
        /// <summary>
        /// The sequence number
        /// </summary>
        public long Sequence { get; set; }
        /// <summary>
        /// The bids in this book
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> Bid { get; set; } = new List<BittrexOrderBookEntry>();
        /// <summary>
        /// The asks in this book
        /// </summary>
        public IEnumerable<BittrexOrderBookEntry> Ask { get; set; } = new List<BittrexOrderBookEntry>();

        IEnumerable<ISymbolOrderBookEntry> ICommonOrderBook.CommonBids => Bid;
        IEnumerable<ISymbolOrderBookEntry> ICommonOrderBook.CommonAsks => Ask;
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
        [JsonProperty("Rate")]
        public decimal Price { get; set; }
    }
}
