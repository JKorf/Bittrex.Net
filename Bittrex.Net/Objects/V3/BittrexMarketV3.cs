using System;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Market info
    /// </summary>
    public class BittrexMarketV3
    {
        /// <summary>
        /// The symbol of the market
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// The base currency of the market
        /// </summary>
        [JsonProperty("baseCurrencySymbol")]
        public string BaseCurrency { get; set; }
        /// <summary>
        /// The quote currency of the market
        /// </summary>
        [JsonProperty("quoteCurrencySymbol")]
        public string QuoteCurrency { get; set; }
        /// <summary>
        /// The minimum trade size for this market
        /// </summary>
        public decimal MinTradeSize { get; set; }
        /// <summary>
        /// The max pricision for this market
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// The status of the market
        /// </summary>
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// When the market was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Additional info
        /// </summary>
        public string Notice { get; set; }
    }
}
