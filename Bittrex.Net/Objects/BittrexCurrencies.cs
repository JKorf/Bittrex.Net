using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexCurrencies
    {
        public string Currency { get; set; }
        public string CurrencyLong { get; set; }
        public int MinConfirmation { get; set; }
        [JsonProperty("txFee")]
        public double TransactionFee { get; set; }
        public bool IsActive { get; set; }
        public string CoinType { get; set; }
        public string BaseAddress { get; set; }
    }
}
