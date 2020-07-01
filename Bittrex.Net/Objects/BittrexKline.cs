using Newtonsoft.Json;
using System;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Bittrex kline
    /// </summary>
    public class BittrexKline
    {
        /// <summary>
        /// Open price of the kline
        /// </summary>
        [JsonProperty("O")]
        public decimal Open { get; set; }
        /// <summary>
        /// High price of the kline
        /// </summary>
        [JsonProperty("H")]
        public decimal High { get; set; }
        /// <summary>
        /// Low price of the kline
        /// </summary>
        [JsonProperty("L")]
        public decimal Low { get; set; }
        /// <summary>
        /// Close price of the kline
        /// </summary>
        [JsonProperty("C")]
        public decimal Close { get; set; }
        /// <summary>
        /// Volume of the kline
        /// </summary>
        [JsonProperty("V")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Base volume of the kline
        /// </summary>
        [JsonProperty("BV")]
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the kline
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
