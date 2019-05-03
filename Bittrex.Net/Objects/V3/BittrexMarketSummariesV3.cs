using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketSummariesV3
    {
        public string Symbol { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal BaseVolume { get; set; }
        public decimal PercentChange { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
