using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexWithdrawal
    {
        public Guid PaymentUuid { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string Address { get; set; }
        public DateTime Opened { get; set; }
        public bool Authorized { get; set; }
        public bool PendingPayment { get; set; }
        [JsonProperty("TxCost")]
        public double TransactionCost { get; set; }
        [JsonProperty("TxId")]
        public string TransactionId { get; set; }
        public bool Canceled { get; set; }
        public bool InvalidAddress { get; set; }
    }
}
