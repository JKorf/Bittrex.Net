using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamExchangeState
    {
        public long Nounce { get; set; }
        public List<BittrexOrderBookEntry> Buys { get; set; }
        public List<BittrexOrderBookFill> Fills { get; set; }
        public List<BittrexOrderBookEntry> Sells { get; set; }


        public String MarketName { get; set; }
    }

    public class BittrexOrderBookFill
    {
        /// <summary>
        /// Total quantity of order at this price
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the orders
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Sell/buy
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public String MarketName { get; set; }

        public override string ToString()
        {
            return "BittrexOrderBookFill: " + OrderType + " for " + MarketName + " " + Quantity + "\t@\t" + Rate;
        }

    }
}
