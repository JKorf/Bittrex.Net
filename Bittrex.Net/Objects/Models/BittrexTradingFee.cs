using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Trading fee info
    /// </summary>
    public class BittrexTradingFee
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Maker rate
        /// </summary>
        public decimal MakerRate { get; set; }
        /// <summary>
        /// Taker rate
        /// </summary>
        public decimal TakerRate { get; set; }
    }
}
