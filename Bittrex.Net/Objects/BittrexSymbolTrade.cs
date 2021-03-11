using System;
using Bittrex.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Info on a trade
    /// </summary>
    public class BittrexSymbolTrade: ICommonRecentTrade
    {
        /// <summary>
        /// Unique id of the trade
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// Side of the taker
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide TakerSide { get; set; }
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the entry
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// The timestamp of the trade execution
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        decimal ICommonRecentTrade.CommonPrice => Rate;
        decimal ICommonRecentTrade.CommonQuantity => Quantity;
        DateTime ICommonRecentTrade.CommonTradeTime => ExecutedAt;
    }
}
