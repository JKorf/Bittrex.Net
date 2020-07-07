using System;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Symbol kline info
    /// </summary>
    public class BittrexKlineV3
    {
        /// <summary>
        /// The opening time of this kline
        /// </summary>
        public DateTime StartsAt { get; set; }
        /// <summary>
        /// The price at opening
        /// </summary>
        public decimal Open { get; set; }
        /// <summary>
        /// The price at closing
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// The volume during this kline
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The quote volume of this candle
        /// </summary>
        public decimal QuoteVolume { get; set; }
    }
}
