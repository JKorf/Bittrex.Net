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
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price in the last 24 hour
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// The volume in the last 24 hour in the market currency 
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The latest price
        /// </summary>
        public decimal Last { get; set; }
        /// <summary>
        /// The base volume in the last 24 hour in the base currency
        /// </summary>
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the summary
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The highest bid
        /// </summary>
        public decimal Bid { get; set; }
        /// <summary>
        /// The lowest ask
        /// </summary>
        public decimal Ask { get; set; }
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
        public decimal PrevDay { get; set; }
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
