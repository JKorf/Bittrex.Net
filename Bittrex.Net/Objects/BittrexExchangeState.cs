using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexExchangeState
    {
        public long Nounce { get; set; }

        /// <summary>
        /// Buys in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Buys { get; set; }

        /// <summary>
        /// Market history
        /// </summary>
        public List<BittrexMarketHistory> Fills { get; set; }

        /// <summary>
        /// Sells in the order book
        /// </summary>
        public List<BittrexOrderBookEntry> Sells { get; set; }

        /// <summary>
        /// Name of the market
        /// </summary>
        public String MarketName { get; set; }
    }
}
