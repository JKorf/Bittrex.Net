using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public abstract class BittrexExchangeState
    {
        public long Nounce { get; set; }

        /// <summary>
        /// Name of the market
        /// </summary>
        public string MarketName { get; set; }

        /// <summary>
        /// Buys in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Buys { get; set; }

        /// <summary>
        /// Sells in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Sells { get; set; }
    }

    public class BittrexStreamUpdateExchangeState: BittrexExchangeState
    {
        /// <summary>
        /// Market history
        /// </summary>
        public List<BittrexStreamFill> Fills { get; set; }
    }

    public class BittrexStreamQueryExchangeState : BittrexExchangeState
    {
        /// <summary>
        /// Market history
        /// </summary>
        public List<BittrexMarketHistory> Fills { get; set; }
    }
}
