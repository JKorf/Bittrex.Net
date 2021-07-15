using Bittrex.Net.Converters;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Kline update data
    /// </summary>
    public class BittrexKlineUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Symbol of the update
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Interval
        /// </summary>
        [JsonConverter(typeof(KlineIntervalConverter))]
        public KlineInterval Interval { get; set; }

        /// <summary>
        /// Kline data
        /// </summary>
        public BittrexKline Delta { get; set; } = default!;
    }
}
