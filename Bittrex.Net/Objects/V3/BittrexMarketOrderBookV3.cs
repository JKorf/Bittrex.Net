using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketOrderBookV3
    {
        public BittrexOrderBookEntryV3[] Bid { get; set; }
        public BittrexOrderBookEntryV3[] Ask { get; set; }
    }

    public class BittrexOrderBookEntryV3
    {
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
    }

    public class BittrexMarketTradeV3 : BittrexOrderBookEntryV3
    {
        public DateTime ExecutedAt { get; set; }
    }
}
