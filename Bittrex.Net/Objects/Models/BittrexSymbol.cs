using System;
using System.Collections.Generic;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public class BittrexSymbol
    {
        /// <summary>
        /// The symbol of the symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The base asset of the symbol
        /// </summary>
        [JsonProperty("baseCurrencySymbol")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// The quote asset of the symbol
        /// </summary>
        [JsonProperty("quoteCurrencySymbol")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// The minimum trade quantity for this symbol
        /// </summary>
        [JsonProperty("minTradeSize")]
        public decimal MinTradeQuantity { get; set; }
        /// <summary>
        /// The max precision for this symbol
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// When the symbol was created
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Additional info
        /// </summary>
        public string Notice { get; set; } = string.Empty;
        /// <summary>
        /// List of prohibited regions. empty if its not restricted.
        /// </summary>
        public IEnumerable<string> ProhibitedIn { get; set; } = Array.Empty<string>();
        /// <summary>
        /// List of associated terms of service.
        /// </summary>
        public IEnumerable<string> AssociatedTermsOfService { get; set; } = Array.Empty<string>();
        /// <summary>
        /// List of tags
        /// </summary>
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
    }
}
