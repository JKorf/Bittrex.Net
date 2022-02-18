using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
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
        /// The asset of the deposit address
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// the address
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = string.Empty;
    }
}
