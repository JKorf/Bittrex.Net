using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexBalanceV3
    {
        [JsonProperty("currencySymbol")]
        public string Currency { get; set; }
        public decimal Total { get; set; }
        public decimal Available { get; set; }
    }
}
