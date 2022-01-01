using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public class BittrexBalance
    {
        /// <summary>
        /// The asset
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The total funds
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// The available funds
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdateTime { get; set; }
    }
}
