using System;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketOrderBookV3
    {
        /// <summary>
        /// The bids in this book
        /// </summary>
        public BittrexOrderBookEntryV3[] Bid { get; set; }
        /// <summary>
        /// The asks in this book
        /// </summary>
        public BittrexOrderBookEntryV3[] Ask { get; set; }
    }

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

    public class BittrexMarketTradeV3 : BittrexOrderBookEntryV3
    {
        /// <summary>
        /// The timestamp of the trade execution
        /// </summary>
        public DateTime ExecutedAt { get; set; }
    }
}
