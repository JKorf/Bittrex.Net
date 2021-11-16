using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Withdrawal info
    /// </summary>
    public class BittrexWithdrawal
    {
        /// <summary>
        /// The id of the withdrawal
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// The asset of the withdrawal
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the withdrawal
        /// </summary>
        public decimal Quantity { get; set; }        
        /// <summary>
        /// Payment method id
        /// </summary>
        public string FundsTransferMethodId { get; set; } = string.Empty;
        /// <summary>
        /// The address the withdrawal is to
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = string.Empty;
        /// <summary>
        /// The transaction cost of the withdrawal
        /// </summary>
        [JsonProperty("txCost")]
        public decimal TransactionFee { get; set; }
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonProperty("txId")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// The status of the withdrawal
        /// </summary>
        [JsonConverter(typeof(WithdrawalStatusConverter))]
        public WithdrawalStatus Status { get; set; }
        /// <summary>
        /// The time the withdrawal was created
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// The time the withdrawal was completed
        /// </summary>
        [JsonProperty("completedAt")]
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// Withdrawal id as specified by the client
        /// </summary>
        public string ClientWithdrawalId { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal target
        /// </summary>
        public string Target { get; set; } = string.Empty;
        /// <summary>
        /// Account id
        /// </summary>
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Error info
        /// </summary>
        public BittrexError? Error { get; set; }
    }
}
