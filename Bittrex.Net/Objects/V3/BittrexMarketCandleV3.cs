using System;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Market candle info
    /// </summary>
    public class BittrexMarketCandleV3
    {
        /// <summary>
        /// The opening time of this candle
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
        /// The highest price during this candle
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price during this candle
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// The volume during this candle
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The volume of this candle in the base currency
        /// </summary>
        public decimal BaseVolume { get; set; }
    }
}
