using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketTickV3
    {
        public decimal LastTradeRate { get; set; }
        public decimal BidRate { get; set; }
        public decimal AskRate { get; set; }
    }
}
