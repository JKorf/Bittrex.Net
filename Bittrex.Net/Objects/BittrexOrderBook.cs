using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexOrderBook
    {
        public List<BittrexOrderBookEntry> Buy { get; set; }
        public List<BittrexOrderBookEntry> Sell { get; set; }
    }

    public class BittrexOrderBookEntry
    {
        public double Quantity { get; set; }
        public double Rate { get; set; }
    }
}
