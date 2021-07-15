using System;
using System.Linq;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Trade execution
    /// </summary>
    public class BittrexExecution: ICommonTrade
    {
        /// <summary>
        /// Id of the execution
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
        public DateTime ExecutedAt { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Rate
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// Id of the order
        /// </summary>
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Paid commission
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// Is taker
        /// </summary>
        public bool IsTaker { get; set; }

        string ICommonTrade.CommonId => Id;
        decimal ICommonTrade.CommonPrice => Rate;
        decimal ICommonTrade.CommonQuantity => Quantity;
        decimal ICommonTrade.CommonFee => Commission;
        string ICommonTrade.CommonFeeAsset => Symbol.Split('-').Last();
        DateTime ICommonTrade.CommonTradeTime => ExecutedAt;
    }
}
