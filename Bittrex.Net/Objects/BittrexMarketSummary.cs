using System;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// 24 hour summary of a market
    /// </summary>
    public class BittrexMarketSummary
    {
        /// <summary>
        /// The name of the market
        /// </summary>
        public string MarketName { get; set; }
        /// <summary>
        /// The highest price in the last 24 hour
        /// </summary>
        public double High { get; set; }
        /// <summary>
        /// The lowest price in the last 24 hour
        /// </summary>
        public double Low { get; set; }
        /// <summary>
        /// The volume in the last 24 hour in the market currency 
        /// </summary>
        public double Volume { get; set; }
        /// <summary>
        /// The latest price
        /// </summary>
        public double Last { get; set; }
        /// <summary>
        /// The base volume in the last 24 hour in the base currency
        /// </summary>
        public double BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the summary
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The highest bid
        /// </summary>
        public double Bid { get; set; }
        /// <summary>
        /// The lowest ask
        /// </summary>
        public double Ask { get; set; }
        /// <summary>
        /// Current open buy orders
        /// </summary>
        public int OpenBuyOrders { get; set; }
        /// <summary>
        /// Current open sell orders
        /// </summary>
        public int OpenSellOrders { get; set; }
        /// <summary>
        /// Price 24 hours ago
        /// </summary>
        public double PrevDay { get; set; }
        /// <summary>
        /// Timestamp when created
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Alternative display market name
        /// </summary>
        public string DisplayMarketName { get; set; }
    }
}
