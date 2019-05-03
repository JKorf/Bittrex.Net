using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketCandleV3
    {
        public DateTime StartsAt { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal BaseVolume { get; set; }
    }
}
