using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexCurrencyV3
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string CoinType { get; set; }
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        public int MinConfirmations { get; set; }
        public string Notice { get; set; }
        [JsonProperty("txFee")]
        public decimal TransactionFee { get; set; }
    }
}
