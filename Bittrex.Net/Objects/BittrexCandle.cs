using Newtonsoft.Json;
using System;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Objects
{
    public class BittrexCandle
    {
        /// <summary>
        /// Open price of the candle
        /// </summary>
        [JsonProperty("O")]
        public decimal Open { get; set; }
        /// <summary>
        /// High price of the candle
        /// </summary>
        [JsonProperty("H")]
        public decimal High { get; set; }
        /// <summary>
        /// Low price of the candle
        /// </summary>
        [JsonProperty("L")]
        public decimal Low { get; set; }
        /// <summary>
        /// Close price of the candle
        /// </summary>
        [JsonProperty("C")]
        public decimal Close { get; set; }
        /// <summary>
        /// Volume of the candle
        /// </summary>
        [JsonProperty("V")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Base volume of the candle
        /// </summary>
        [JsonProperty("BV")]
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the candle
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
