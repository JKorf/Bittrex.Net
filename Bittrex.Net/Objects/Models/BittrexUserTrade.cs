﻿using System;
using System.Linq;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// User trade info
    /// </summary>
    public class BittrexUserTrade: ICommonTrade
    {
        /// <summary>
        /// Id of the trade
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// The symbol of the execution
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of execution
        /// </summary>
        [JsonProperty("executedAt")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Execution quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Execution price
        /// </summary>
        [JsonProperty("rate")]
        public decimal Price { get; set; }
        /// <summary>
        /// Id of the order
        /// </summary>
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Paid fee
        /// </summary>
        [JsonProperty("commission")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Is taker
        /// </summary>
        public bool IsTaker { get; set; }

        string ICommonTrade.CommonId => Id;
        decimal ICommonTrade.CommonPrice => Price;
        decimal ICommonTrade.CommonQuantity => Quantity;
        decimal ICommonTrade.CommonFee => Fee;
        string ICommonTrade.CommonFeeAsset => Symbol.Split('-').Last();
        DateTime ICommonTrade.CommonTradeTime => Timestamp;
    }
}
