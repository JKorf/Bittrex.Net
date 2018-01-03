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
        /// TODO: use ENUM
        /// </summary>
        public String OrderType { get; set; }

        /// <summary>
        /// TimeStamp=2018-01-03T02:38:19.927
        /// TODO: use DateTime object
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return "BittrexOrderBookFill: " + OrderType + " " + Quantity + "\t@\t" + Rate;
        }

    }
}
