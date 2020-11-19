using System;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Withdrawal info
    /// </summary>
    public class BittrexWithdrawalV3
    {
        /// <summary>
        /// The id of the withdrawal
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The currency of the withdrawal
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; } = "";
        /// <summary>
        /// The quantity of the withdrawal
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The address the withdrawal is to
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = "";
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = "";
        /// <summary>
        /// The transaction cost of the withdrawal
        /// </summary>
        [JsonProperty("txCost")]
        public decimal TransactionCost { get; set; }
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonProperty("txId")]
        public string TransactionId { get; set; } = "";
        /// <summary>
        /// The status of the withdrawal
        /// </summary>
        [JsonConverter(typeof(WithdrawalStatusConverter))]
        public WithdrawalStatus Status { get; set; }
        /// <summary>
        /// The time the withdrawal was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// The time the withdrawal was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Withdrawal id as specified by the client
        /// </summary>
        public string ClientWithdrawalId { get; set; } = "";
    }
}
