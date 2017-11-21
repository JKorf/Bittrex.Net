using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a deposit
    /// </summary>
    public class BittrexDeposit
    {
        /// <summary>
        /// The id of the deposit
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The amount of the deposit
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The currency of the deposit
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The current number of confirmations the deposit has
        /// </summary>
        public int Confirmations { get; set; }
        /// <summary>
        /// Timestamp of the last update
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Transaction id of the deposit
        /// </summary>
        [JsonProperty("TxId")]
        public string TransactionId { get; set; }
        /// <summary>
        /// The address the deposit is to
        /// </summary>
        public string CryptoAddress { get; set; }
    }
}
