using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Converters.V3;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.V3;
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
        public string Symbol { get; set; }
        /// <summary>
        /// Interval
        /// </summary>
        [JsonConverter(typeof(KlineIntervalConverter))]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// Kline data
        /// </summary>
        public BittrexKlineV3 Delta { get; set; }
    }
}
