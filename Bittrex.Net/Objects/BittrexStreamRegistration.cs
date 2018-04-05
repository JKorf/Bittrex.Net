using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public abstract class BittrexRegistration
    {
        public int StreamId { get; set; }
    }

    internal class BittrexMarketSummariesRegistration: BittrexRegistration
    {
        public Action<List<BittrexStreamMarketSummary>> Callback { get; set; }
    }

    internal class BittrexMarketSummariesLiteRegistration : BittrexRegistration
    {
        public Action<List<BittrexStreamMarketSummaryLite>> Callback { get; set; }
    }

    internal class BittrexExchangeStateRegistration : BittrexRegistration
    {
        public Action<BittrexStreamUpdateExchangeState> Callback { get; set; }
        public string MarketName { get; set; }
    }

    internal class BittrexBalanceUpdateRegistration : BittrexRegistration
    {
        public Action<BittrexStreamBalance> Callback { get; set; }
    }

    internal class BittrexOrderUpdateRegistration : BittrexRegistration
    {
        public Action<BittrexStreamOrderData> Callback { get; set; }
    }
}
