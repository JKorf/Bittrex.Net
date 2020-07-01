﻿using System;
using Bittrex.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about an order executed on a symbol
    /// </summary>
    public class BittrexSymbolTrade
    {
        /// <summary>
        /// The order id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Timestamp of the order
        /// </summary>
        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Total price of the order
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// Whether the order was fully filled
        /// </summary>
        [JsonConverter(typeof(FillTypeConverter))]
        public FillType FillType { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide OrderType { get; set; }
    }
}
