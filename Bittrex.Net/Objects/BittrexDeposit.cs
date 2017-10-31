using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexDeposit
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public int Confirmations { get; set; }
        public DateTime LastUpdated { get; set; }
        [JsonProperty("TxId")]
        public string TransactionId { get; set; }
        public string CryptoAddress { get; set; }
    }
}
