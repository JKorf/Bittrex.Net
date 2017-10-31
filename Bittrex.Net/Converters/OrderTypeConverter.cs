using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class OrderTypeConverter: JsonConverter
    {
        private readonly bool quotes;

        public OrderTypeConverter()
        {
            quotes = true;
        }

        public OrderTypeConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<OrderType, string> values = new Dictionary<OrderType, string>()
        {
            { OrderType.Buy, "BUY" },
            { OrderType.Sell, "SELL" }
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(OrderType)value]);
            else
                writer.WriteRawValue(values[(OrderType)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderType);
        }
    }
}
