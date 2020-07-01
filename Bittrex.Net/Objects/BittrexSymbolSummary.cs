﻿using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// 24 hour summary of a symbol
    /// </summary>
    public class BittrexSymbolSummary
    {
        /// <summary>
        /// The name of the symbol
        /// </summary>
        [JsonProperty("marketName")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The highest price in the last 24 hour
        /// </summary>
        public decimal? High { get; set; }
        /// <summary>
        /// The lowest price in the last 24 hour
        /// </summary>
        public decimal? Low { get; set; }
        /// <summary>
        /// The volume in the last 24 hour in the quote currency 
        /// </summary>
        public decimal? Volume { get; set; }
        /// <summary>
        /// The latest price
        /// </summary>
        public decimal? Last { get; set; }
        /// <summary>
        /// The base volume in the last 24 hour in the base currency
        /// </summary>
        public decimal? BaseVolume { get; set; }
        /// <summary>
        /// Timestamp of the summary
        /// </summary>
        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The highest bid
        /// </summary>
        public decimal? Bid { get; set; }
        /// <summary>
        /// The lowest ask
        /// </summary>
        public decimal? Ask { get; set; }
        /// <summary>
        /// Current open buy orders
        /// </summary>
        public int? OpenBuyOrders { get; set; }
        /// <summary>
        /// Current open sell orders
        /// </summary>
        public int? OpenSellOrders { get; set; }
        /// <summary>
        /// Price 24 hours ago
        /// </summary>
        public decimal? PrevDay { get; set; }
        /// <summary>
        /// Timestamp when created
        /// </summary>
        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Created { get; set; }
    }
}
