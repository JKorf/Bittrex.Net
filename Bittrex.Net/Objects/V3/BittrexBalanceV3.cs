using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexBalanceV3
    {
        /// <summary>
        /// The currency
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; }
        /// <summary>
        /// The total funds
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// The available funds
        /// </summary>
        public decimal Available { get; set; }
    }
}
