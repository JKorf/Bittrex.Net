using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream exchange state update
    /// </summary>
    public class BittrexStreamUpdateExchangeState
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// Name of the market
        /// </summary>
        [JsonProperty("M")]
        public string MarketName { get; set; } = "";

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
        /// Market history
        /// </summary>
        [JsonProperty("f")]
        public IEnumerable<BittrexStreamFill> Fills { get; set; } = new List<BittrexStreamFill>();
    }

    /// <summary>
    /// Stream query state
    /// </summary>
    public class BittrexStreamQueryExchangeState
    {
        /// <summary>
        /// Nonce
        /// </summary>
        [JsonProperty("N")]
        public long Nonce { get; set; }

        /// <summary>
        /// Name of the market
        /// </summary>
        [JsonProperty("M")]
        public string MarketName { get; set; } = "";

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
        /// Market history
        /// </summary>
        [JsonProperty("f")]
        public IEnumerable<BittrexStreamMarketHistory> Fills { get; set; } = new List<BittrexStreamMarketHistory>();
    }
}
