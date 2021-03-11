using System;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Conditional order info
    /// </summary>
    public class BittrexConditionalOrder
    {
        /// <summary>
        /// Id of the order
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; } = "";
        /// <summary>
        /// Operand type
        /// </summary>
        public string Operand { get; set; } = "";
        /// <summary>
        /// Percent above the minimum price (GTE) or below the maximum price (LTE) at which to trigger
        /// </summary>
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// The stop price will automatically adjust relative to the most extreme trade value seen.
        /// </summary>
        public decimal? TrailingStopPercent { get; set; }
        /// <summary>
        /// Unique ID of the order that was created by this conditional, if there is one
        /// </summary>
        public string? CreatedOrderId { get; set; }
        /// <summary>
        /// Order to create if this conditional order is triggered
        /// </summary>
        public BittrexUnplacedOrder? OrderToCreate { get; set; }
        /// <summary>
        /// Order or conditional order to cancel if this conditional order triggers
        /// </summary>
        public BittrexLinkedOrder? OrderToCancel { get; set; }
        /// <summary>
        /// Client-provided identifier for idempotency
        /// </summary>
        public string? ClientConditionalOrderId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; } = "";
        /// <summary>
        /// If a conditional order fails to create an order when triggered, the failure reason will appear here
        /// </summary>
        public string? OrderCreationErrorCode { get; set; }
        /// <summary>
        /// Order creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Timestamp of last update
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Timestamp order was closed
        /// </summary>
        public DateTime? ClosedAt { get; set; }
    }
}
