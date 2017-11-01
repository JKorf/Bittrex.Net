using System;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamRegistration
    {
        public Action<BittrexMarketSummary> Callback { get; set; }
        public string MarketName { get; set; }
        public int StreamId { get; set; }
    }
}
