using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Deposit info
    /// </summary>
    public class BittrexDeposit
    {
        /// <summary>
        /// The id of the deposit
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// The asset of the deposit
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the deposit
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Payment method id
        /// </summary>
        public string FundsTransferMethodId { get; set; } = string.Empty;
        /// <summary>
        /// The address of the deposit
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; } = string.Empty;
        /// <summary>
        /// The transaction id of the deposit
        /// </summary>
        [JsonProperty("txId")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// The current amount of confirmations
        /// </summary>
        public int Confirmations { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Completed time
        /// </summary>
        [JsonProperty("completedAt")]
        public DateTime? CompleteTime { get; set; }
        /// <summary>
        /// The status of the deposit
        /// </summary>
        [JsonConverter(typeof(DepositStatusConverter))]
        public DepositStatus Status { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        public string Source { get; set; } = string.Empty;
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
