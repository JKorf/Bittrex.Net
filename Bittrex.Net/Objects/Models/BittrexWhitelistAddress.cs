using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Whitelist address
    /// </summary>
    public class BittrexWhitelistAddress
    {
        /// <summary>
        /// The asset of the address
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// When the address was whitelisted
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Status of the address
        /// </summary>
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// When it may be used for withdrawals
        /// </summary>
        [JsonProperty("activeAt")]
        public DateTime ActiveTime { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Tag for the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = string.Empty;
    }
}
