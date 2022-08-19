using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Fiat fee info
    /// </summary>
    public class BittrexFiatFee
    {
        /// <summary>
        /// Asset name
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Fee value
        /// </summary>
        public decimal Fees { get; set; }
        /// <summary>
        /// Transaction type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// Transfer type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public TransferType TransferType { get; set; }
        /// <summary>
        /// Fee type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public FeeType FeeType { get; set; }
    }
}
