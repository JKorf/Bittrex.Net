using System;
using System.Collections.Generic;
using Bittrex.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public class BittrexSymbol: ICommonSymbol
    {
        /// <summary>
        /// The symbol of the symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The base currency of the symbol
        /// </summary>
        [JsonProperty("baseCurrencySymbol")]
        public string BaseCurrency { get; set; } = string.Empty;
        /// <summary>
        /// The quote currency of the symbol
        /// </summary>
        [JsonProperty("quoteCurrencySymbol")]
        public string QuoteCurrency { get; set; } = string.Empty;
        /// <summary>
        /// The minimum trade size for this symbol
        /// </summary>
        public decimal MinTradeSize { get; set; }
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
        public DateTime CreatedAt { get; set; }
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

        string ICommonSymbol.CommonName => Symbol;
        decimal ICommonSymbol.CommonMinimumTradeSize => MinTradeSize;
    }
}
