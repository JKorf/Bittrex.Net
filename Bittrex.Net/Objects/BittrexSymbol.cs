using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a symbol
    /// </summary>
    public class BittrexSymbol
    {
        /// <summary>
        /// The quote currency
        /// </summary>
        [JsonProperty("marketCurrency")]
        public string QuoteCurrency { get; set; } = "";
        /// <summary>
        /// The base currency
        /// </summary>
        public string BaseCurrency { get; set; } = "";
        /// <summary>
        /// The long name of the quote currency
        /// </summary>
        [JsonProperty("marketCurrencyLong")]
        public string QuoteCurrencyLong { get; set; } = "";
        /// <summary>
        /// The long name of the base currency
        /// </summary>
        public string BaseCurrencyLong { get; set; } = "";
        /// <summary>
        /// The minimun size of an order
        /// </summary>
        public decimal MinTradeSize { get; set; }
        /// <summary>
        /// The name of the symbol
        /// </summary>
        [JsonProperty("marketName")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Whether the symbol is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Timestamp when the symbol was created
        /// </summary>
        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Created { get; set; }
        /// <summary>
        /// Additional information about the state of this symbol
        /// </summary>
        public string Notice { get; set; } = "";
        /// <summary>
        /// Whether the symbol is sponsored by Bittrex
        /// </summary>
        public bool? IsSponsored { get; set; }
        /// <summary>
        /// Url of the logo
        /// </summary>
        public string LogoUrl { get; set; } = "";
    }
}
