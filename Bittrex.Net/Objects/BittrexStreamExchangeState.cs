using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream exchange state update
    /// </summary>
    public class BittrexStreamOrderBookUpdate
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// Name of the symbol
        /// </summary>
        [JsonProperty("M")]
        public string Symbol { get; set; } = "";

        /// <summary>
        /// Buys in the order book
        /// </summary>
        [JsonProperty("Z")]
        public IEnumerable<BittrexStreamOrderBookUpdateEntry> Buys { get; set; } = new List<BittrexStreamOrderBookUpdateEntry>();

        /// <summary>
        /// Sells in the order book
        /// </summary>
        [JsonProperty("S")]
        public IEnumerable<BittrexStreamOrderBookUpdateEntry> Sells { get; set; } = new List<BittrexStreamOrderBookUpdateEntry>();
        /// <summary>
        /// Symbol history
        /// </summary>
        [JsonProperty("f")]
        public IEnumerable<BittrexStreamFill> Fills { get; set; } = new List<BittrexStreamFill>();
    }

    /// <summary>
    /// Stream order book state
    /// </summary>
    public class BittrexStreamOrderBook
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// Name of the symbol
        /// </summary>
        [JsonProperty("M")]
        public string Symbol { get; set; } = "";

        /// <summary>
        /// Buys in the order book
        /// </summary>
        [JsonProperty("Z")]
        public IEnumerable<BittrexStreamOrderBookEntry> Buys { get; set; } = new List<BittrexStreamOrderBookEntry>();

        /// <summary>
        /// Sells in the order book
        /// </summary>
        [JsonProperty("S")]
        public IEnumerable<BittrexStreamOrderBookEntry> Sells { get; set; } = new List<BittrexStreamOrderBookEntry>();
        /// <summary>
        /// Symbol history
        /// </summary>
        [JsonProperty("f")]
        public IEnumerable<BittrexStreamSymbolTrade> Fills { get; set; } = new List<BittrexStreamSymbolTrade>();
    }
}
