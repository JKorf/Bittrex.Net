using System;
using System.Collections.Generic;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Bittrex currency info
    /// </summary>
    public class BittrexCurrency
    {
        /// <summary>
        /// The symbol for this currency
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The full name of the currency
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The type of the currency
        /// </summary>
        public string CoinType { get; set; } = string.Empty;
        /// <summary>
        /// The status of the currency
        /// </summary>
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// The minimal amount of confirmation that is needed before the funds of a deposit are credited to the account
        /// </summary>
        public int MinConfirmations { get; set; }
        /// <summary>
        /// Additional info
        /// </summary>
        public string Notice { get; set; } = string.Empty;
        /// <summary>
        /// The transaction fee
        /// </summary>
        [JsonProperty("txFee")]
        public decimal TransactionFee { get; set; }
        /// <summary>
        /// Url to the logo
        /// </summary>
        public string LogoUrl { get; set; } = string.Empty;
        /// <summary>
        /// List of prohibited regions. empty if its not restricted.
        /// </summary>
        public IEnumerable<string> ProhibitedIn { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Base address of the currency
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// List of associated terms of service.
        /// </summary>
        public IEnumerable<string> AssociatedTermsOfService { get; set; } = Array.Empty<string>();
    }
}
