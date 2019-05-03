using System;
using Bittrex.Net.Converters;
using Bittrex.Net.Converters.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects.V3
{
    public class BittrexOrderV3
    {
        public string Id { get; set; }
        [JsonProperty("marketSymbol")]
        public string Symbol { get; set; }
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Direction { get; set; }
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderTypeV3 Type { get; set; }
        public decimal Quantity { get; set; }
        public decimal Limit { get; set; }
        public decimal Ceiling { get; set; }
        [JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce TimeInForce { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string ClientOrderId { get; set; }
        public decimal FillQuantity { get; set; }
        public decimal Commission { get; set; }
        public decimal Proceeds { get; set; }
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ClosedAt { get; set; }

    }
}
