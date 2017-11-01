using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamDeltas
    {
        public long Nounce { get; set; }
        public List<BittrexMarketSummary> Deltas { get; set; }
    }
}
