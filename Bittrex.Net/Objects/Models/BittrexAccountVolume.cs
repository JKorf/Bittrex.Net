using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Volume info
    /// </summary>
    public class BittrexAccountVolume
    {
        /// <summary>
        /// Update time
        /// </summary>
        [JsonProperty("updated")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Volume 30 days
        /// </summary>
        public decimal Volume30Days { get; set; }
    }
}
