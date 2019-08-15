using System;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Deposit info
    /// </summary>
    public class BittrexDepositV3
    {
        /// <summary>
        /// The id of the deposit
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The currency of the deposit
        /// </summary>
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; }
        /// <summary>
        /// The quantity of the deposit
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The address of the deposit
        /// </summary>
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; }
        /// <summary>
        /// The tag of the address
        /// </summary>
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; }
        /// <summary>
        /// The transaction id of the deposit
        /// </summary>
        [JsonProperty("txId")]
        public string TransactionId { get; set; }
        /// <summary>
        /// The current amount of confirmations
        /// </summary>
        public int Confirmations { get; set; }
        /// <summary>
        /// The timestamp of the last update
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// The timestamp of when this deposit was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }
        /// <summary>
        /// The status of the deposit
        /// </summary>
        [JsonConverter(typeof(DepositStatusConverter))]
        public DepositStatus Status { get; set; }
    }
}
