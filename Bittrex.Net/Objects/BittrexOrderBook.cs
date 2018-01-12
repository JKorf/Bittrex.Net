using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Order book for a market
    /// </summary>
    public class BittrexOrderBook
    {
        /// <summary>
        /// List of buy orders in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Buy { get; set; }
        /// <summary>
        /// List of sell orders in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Sell { get; set; }
    }

    public class BittrexOrderBookEntry
    {
        /// <summary>
        /// Total quantity of order at this price
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the orders
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// how to handle data (used by stream)
        /// </summary>
        public OrderBookEntryType Type { get; set; }
    }

    /// <summary>
    /// https://github.com/JKorf/Bittrex.Net/pull/42#discussion_r160122966
    /// Type 0 – you need to add this entry into your orderbook. There were no orders at matching price before.
    /// Type 1 – you need to delete this entry from your orderbook.This entry no longer exists (no orders at matching price)
    /// Type 2 – you need to edit this entry.There are different number of orders at this price.
    /// </summary>
    public enum OrderBookEntryType
    {
        NewEntry = 0,
        RemoveEntry = 1,
        UpdateEntry = 2
    }
}
