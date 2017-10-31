using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class OrderTypeExtendedConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderTypeExtendedConverter()
        {
            quotes = true;
        }

        public OrderTypeExtendedConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<OrderTypeExtended, string> values = new Dictionary<OrderTypeExtended, string>()
        {
            { OrderTypeExtended.LimitBuy, "LIMIT_BUY" },
            { OrderTypeExtended.LimitSell, "LIMIT_SELL" }
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(OrderTypeExtended)value]);
            else
                writer.WriteRawValue(values[(OrderTypeExtended)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderTypeExtended);
        }
    }
}
