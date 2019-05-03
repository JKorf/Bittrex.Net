using System;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexWithdrawalV3
    {
        public string Id { get; set; }
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; }
        public decimal Quantity { get; set; }
        [JsonProperty("cryptoAddress")]
        public string Address { get; set; }
        [JsonProperty("cryptoAddressTag")]
        public string AddressTag { get; set; }
        [JsonProperty("txCost")]
        public decimal TransactionCost { get; set; }
        [JsonProperty("txId")]
        public string TransactionId { get; set; }
        [JsonConverter(typeof(WithdrawalStatusConverter))]
        public WithdrawalStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
