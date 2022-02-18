using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Symbol summary info
    /// </summary>
    public class BittrexSymbolSummary
    {
        /// <summary>
        /// the symbol the summary is for
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The high price for this symbol in the last 24 hours
        /// </summary>
        [JsonProperty("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// The low price for this symbol in the last 24 hours
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Volume within the last 24 hours
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// Quote volume within the last 24 hours
        /// </summary>
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// The percentage change of this symbol for the last 24 hours
        /// </summary>
        public decimal PercentChange { get; set; }
        /// <summary>
        /// The timestamp of when this summary was last updated
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdateTime { get; set; }
    }
}
