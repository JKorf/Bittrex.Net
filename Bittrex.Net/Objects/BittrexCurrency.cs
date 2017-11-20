using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a currency
    /// </summary>
    public class BittrexCurrency
    {
        /// <summary>
        /// The abbreviation of the currency
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The full name of the currency
        /// </summary>
        public string CurrencyLong { get; set; }
        /// <summary>
        /// The minimum number of confirmations before a deposit is added to a account
        /// </summary>
        public int MinConfirmation { get; set; }
        /// <summary>
        /// The transaction fee for a currency
        /// </summary>
        [JsonProperty("txFee")]
        public decimal TransactionFee { get; set; }
        /// <summary>
        /// Whether the currency is currently active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// The base coin type
        /// </summary>
        public string CoinType { get; set; }
        /// <summary>
        /// The base address
        /// </summary>
        public string BaseAddress { get; set; }
    }
}
