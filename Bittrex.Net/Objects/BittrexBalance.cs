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
        /// The currency
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; } = string.Empty;
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
        public DateTime UpdatedAt { get; set; }

        string ICommonBalance.CommonAsset => Currency;
        decimal ICommonBalance.CommonAvailable => Available;
        decimal ICommonBalance.CommonTotal => Total;
    }
}
