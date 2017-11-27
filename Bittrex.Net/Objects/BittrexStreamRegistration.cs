using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    internal abstract class BittrexRegistration
    {
        public int StreamId { get; set; }
    }

    internal class BittrexMarketsAllRegistration: BittrexRegistration
    {
        public Action<List<BittrexMarketSummary>> Callback { get; set; }
    }

    internal class BittrexMarketsRegistration: BittrexRegistration
    {
        public Action<BittrexMarketSummary> Callback { get; set; }
        public string MarketName { get; set; }
    }
}
