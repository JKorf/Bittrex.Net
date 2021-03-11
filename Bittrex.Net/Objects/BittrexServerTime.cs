using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    internal class BittrexServerTime
    {
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime ServerTime { get; set; }
    }
}
