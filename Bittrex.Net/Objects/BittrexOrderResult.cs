using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexOrderResult
    {
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter)), JsonProperty("BuyOrSell")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// The market currency
        /// </summary>
        public string MarketCurrency { get; set; }
        /// <summary>
        /// The market the order is on
        /// </summary>
        public string MarketName { get; set; }
        /// <summary>
        /// The order id of the currency
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// The type of order
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// The quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The rate of the order
        /// </summary>
        public decimal Rate { get; set; }
    }
}
