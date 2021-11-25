using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    internal class BittrexServerTime
    {
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
    }
}
