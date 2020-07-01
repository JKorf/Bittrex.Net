﻿using System;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public class BittrexSymbolV3
    {
        /// <summary>
        /// The symbol of the symbol
        /// </summary>
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The base currency of the symbol
        /// </summary>
        [JsonProperty("baseCurrencySymbol")]
        public string BaseCurrency { get; set; } = "";
        /// <summary>
        /// The quote currency of the symbol
        /// </summary>
        [JsonProperty("quoteCurrencySymbol")]
        public string QuoteCurrency { get; set; } = "";
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
        public string Notice { get; set; } = "";
    }
}
