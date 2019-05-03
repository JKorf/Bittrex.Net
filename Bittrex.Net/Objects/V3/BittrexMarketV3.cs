using System;
using Bittrex.Net.Converters.V3;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexMarketV3
    {
        public string Symbol { get; set; }
        [JsonProperty("baseCurrencySymbol")]
        public string BaseCurrency { get; set; }
        [JsonProperty("quoteCurrencySymbol")]
        public string QuoteCurrency { get; set; }
        public decimal MinTradeSize { get; set; }
        public int Precision { get; set; }
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Notice { get; set; }
    }
}
