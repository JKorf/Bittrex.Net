using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// 24 hour summary of a market
    /// </summary>
    public class BittrexStreamMarketSummary
    {
        /// <summary>
        /// The name of the market
        /// </summary>
        [JsonProperty("M")]
        public string MarketName { get; set; }
        /// <summary>
        /// The highest price in the last 24 hour
        /// </summary>
        [JsonProperty("H")]
        public decimal? High { get; set; }
        /// <summary>
        /// The lowest price in the last 24 hour
        /// </summary>
        [JsonProperty("L")]
        public decimal? Low { get; set; }
        /// <summary>
        /// The volume in the last 24 hour in the market currency 
        /// </summary>
        [JsonProperty("V")]
        public decimal? Volume { get; set; }
        /// <summary>
        /// The latest price
        /// </summary>
        [JsonProperty("l")]
        public decimal? Last { get; set; }
        /// <summary>
        /// The base volume in the last 24 hour in the base currency
        /// </summary>
        [JsonProperty("m")]
        public decimal? BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the summary
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(TimestampConverter))]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The highest bid
        /// </summary>
        [JsonProperty("B")]
        public decimal Bid { get; set; }
        /// <summary>
        /// The lowest ask
        /// </summary>
        [JsonProperty("A")]
        public decimal Ask { get; set; }
        /// <summary>
        /// Current open buy orders
        /// </summary>
        [JsonProperty("G")]
        public int? OpenBuyOrders { get; set; }
        /// <summary>
        /// Current open sell orders
        /// </summary>
        [JsonProperty("g")]
        public int? OpenSellOrders { get; set; }
        /// <summary>
        /// Price 24 hours ago
        /// </summary>
        [JsonProperty("PD")]
        public decimal? PrevDay { get; set; }
        /// <summary>
        /// Timestamp when created
        /// </summary>
        [JsonProperty("x"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Created { get; set; }
    }
}
