﻿using System;
using Bittrex.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Stream fill
    /// </summary>
    public class BittrexStreamFill
    {
		/// <summary>
		/// ID of the fill
		/// </summary>
		[JsonProperty("FI")]
		public long Id { get; set; }
		/// <summary>
		/// Timestamp of the fill
		/// </summary>
		[JsonProperty("T"), JsonConverter(typeof(TimestampConverter))]
		public DateTime Timestamp { get; set; }
		/// <summary>
		/// Quantity of the fill
		/// </summary>
		[JsonProperty("Q")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Rate of the fill
        /// </summary>
        [JsonProperty("R")]
        public decimal Rate { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        [JsonProperty("OT")]
        public OrderSide OrderType { get; set; }
    }
}
