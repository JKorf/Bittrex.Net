using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a withdrawal
    /// </summary>
    public class BittrexWithdrawal
    {
        /// <summary>
        /// Guid of the payment
        /// </summary>
        public Guid PaymentUuid { get; set; }
        /// <summary>
        /// Currency of the withdrawal
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Amount of the withdrawal
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Address the withdrawal is to
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Timestamp when withdrawal was opened
        /// </summary>
        public DateTime Opened { get; set; }
        /// <summary>
        /// Whether the withdrawal is authorized
        /// </summary>
        public bool Authorized { get; set; }
        /// <summary>
        /// Whether there is pending payment
        /// </summary>
        public bool PendingPayment { get; set; }
        /// <summary>
        /// Cost of the transaction
        /// </summary>
        [JsonProperty("TxCost")]
        public decimal TransactionCost { get; set; }
        /// <summary>
        /// Id of the transaction
        /// </summary>
        [JsonProperty("TxId")]
        public string TransactionId { get; set; }
        /// <summary>
        /// Whether the withdrawal is canceled
        /// </summary>
        public bool Canceled { get; set; }
        /// <summary>
        /// Whether the withdrawal is to an invalid address
        /// </summary>
        public bool InvalidAddress { get; set; }
    }
}
