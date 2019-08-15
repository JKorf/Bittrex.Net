using System;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Market summary info
    /// </summary>
    public class BittrexMarketSummariesV3
    {
        /// <summary>
        /// the symbol the summary is for
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// The high price for this symbol in the last 24 hours
        /// </summary>
        public decimal High { get; set; }
        /// <summary>
        /// The low price for this symbol in the last 24 hours
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        /// The volume of this symbol during the last 24 hours
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// The volume of this symbol in the base currency during the last 24 hours
        /// </summary>
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// The percentage change of this symbol for the last 24 hours
        /// </summary>
        public decimal PercentChange { get; set; }
        /// <summary>
        /// The timestamp of when this summary was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
