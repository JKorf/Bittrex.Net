using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Whitelist address
    /// </summary>
    public class BittrexWhitelistAddress
    {
        /// <summary>
        /// The currency of the address
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; } = "";
        /// <summary>
        /// When the address was whitelisted
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Status of the address
        /// </summary>
        public string Status { get; set; } = "";
        /// <summary>
        /// When it may be used for withdrawals
        /// </summary>
        public DateTime ActiveAt { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = "";
        /// <summary>
        /// Tag for the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = "";
    }
}
