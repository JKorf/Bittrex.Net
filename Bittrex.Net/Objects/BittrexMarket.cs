using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a market
    /// </summary>
    public class BittrexMarket
    {
        /// <summary>
        /// The market currency
        /// </summary>
        public string MarketCurrency { get; set; }
        /// <summary>
        /// The base currency
        /// </summary>
        public string BaseCurrency { get; set; }
        /// <summary>
        /// The long name of the market currency
        /// </summary>
        public string MarketCurrencyLong { get; set; }
        /// <summary>
        /// The long name of the base currency
        /// </summary>
        public string BaseCurrencyLong { get; set; }
        /// <summary>
        /// The minimun size of an order
        /// </summary>
        public decimal MinTradeSize { get; set; }
        /// <summary>
        /// The name of the market
        /// </summary>
        public string MarketName { get; set; }
        /// <summary>
        /// Whether the market is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Timestamp when the market was created
        /// </summary>
        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Created { get; set; }
        /// <summary>
        /// Additional information about the state of this market
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// Whether the market is sponsored by Bittrex
        /// </summary>
        public bool? IsSponsored { get; set; }
        /// <summary>
        /// Url of the logo
        /// </summary>
        public string LogoUrl { get; set; }
    }
}
