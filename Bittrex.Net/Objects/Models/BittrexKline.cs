using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Symbol kline info
    /// </summary>
    public class BittrexKline
    {
        /// <summary>
        /// The opening time of this kline
        /// </summary>
        [JsonProperty("startsAt")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// The price at opening
        /// </summary>
        [JsonProperty("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// The price at closing
        /// </summary>
        [JsonProperty("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        [JsonProperty("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice { get; set; }
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
