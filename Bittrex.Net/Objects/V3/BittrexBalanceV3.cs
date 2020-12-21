using Newtonsoft.Json;
using System;
using CryptoExchange.Net.ExchangeInterfaces;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Balance info
    /// </summary>
    public class BittrexBalanceV3: ICommonBalance
    {
        /// <summary>
        /// The currency
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; } = "";
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
