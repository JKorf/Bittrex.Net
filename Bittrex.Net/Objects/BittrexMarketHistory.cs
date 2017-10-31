using System;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexMarketHistory
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        [JsonConverter(typeof(FillTypeConverter))]
        public FillType FillType { get; set; }
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
    }
}
