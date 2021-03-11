using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Deposit address info
    /// </summary>
    public class BittrexDepositAddress
    {
        /// <summary>
        /// The status of the deposit address
        /// </summary>
        [JsonConverter(typeof(DepositAddressStatusConverter))]
        public DepositAddressStatus Status { get; set; }
        /// <summary>
        /// The currency of the deposit address
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; } = "";
        /// <summary>
        /// the address
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = "";
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = "";
    }
}
