using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class OrderBookTypeConverter: JsonConverter
    {
        private readonly bool quotes;

        public OrderBookTypeConverter()
        {
            quotes = true;
        }

        public OrderBookTypeConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<OrderBookType, string> values = new Dictionary<OrderBookType, string>()
        {
            { OrderBookType.Buy, "buy" },
            { OrderBookType.Sell, "sell" },
            { OrderBookType.Both, "both" },
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(OrderBookType)value]);
            else
                writer.WriteRawValue(values[(OrderBookType)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderBookType);
        }
    }
}
