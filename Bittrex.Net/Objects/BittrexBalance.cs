using System;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Balance info
    /// </summary>
    public class BittrexBalance: ICommonBalance
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

        string ICommonBalance.CommonAsset => Asset;
        decimal ICommonBalance.CommonAvailable => Available;
        decimal ICommonBalance.CommonTotal => Total;
    }
}
