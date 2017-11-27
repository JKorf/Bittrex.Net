using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects
{
    public class BittrexCandle
    {
        [JsonProperty("O")]
        public decimal Open { get; set; }
        [JsonProperty("H")]
        public decimal High { get; set; }
        [JsonProperty("L")]
        public decimal Low { get; set; }
        [JsonProperty("C")]
        public decimal Close { get; set; }
        [JsonProperty("V")]
        public decimal Volume { get; set; }
        [JsonProperty("BV")]
        public decimal BaseVolume { get; set; }
        [JsonProperty("T")]
        public DateTime Timestamp { get; set; }
    }
}
