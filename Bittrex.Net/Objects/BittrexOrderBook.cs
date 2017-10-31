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
        public double Quantity { get; set; }
        /// <summary>
        /// Price of the orders
        /// </summary>
        public double Rate { get; set; }
    }
}
