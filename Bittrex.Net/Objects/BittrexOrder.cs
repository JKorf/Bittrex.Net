using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexOrder
    {
        public Guid? Uuid { get; set; }
        public Guid OrderUuid { get; set; }
        public string Exchange { get; set; }
        [JsonConverter(typeof(OrderTypeExtendedConverter))]
        public OrderTypeExtended OrderType { get; set; }
        public double Quantity { get; set; }
        public double QuantityRemaining { get; set; }
        public double Limit { get; set; }
        public double CommissionPaid { get; set; }
        public double Price { get; set; }
        public double? PricePerUnit { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public bool CancelInitiated { get; set; }
        public bool ImmediateOrCancel { get; set; }
        public bool IsConditional { get; set; }
        public string Condition { get; set; }
        public string ConditionTarget { get; set; }
    }
}
