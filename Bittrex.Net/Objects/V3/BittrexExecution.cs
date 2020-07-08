using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Trade execution
    /// </summary>
    public class BittrexExecution
    {
        /// <summary>
        /// Id of the execution
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The symbol of the execution
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Timeestamp of execution
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
        public string OrderId { get; set; } = "";
        /// <summary>
        /// Paid commission
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// Is taker
        /// </summary>
        public bool IsTaker { get; set; }
    }
}
