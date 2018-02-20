using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class OrderSideExtendedConverter : JsonConverter
    {
        private readonly bool quotes;

        public OrderSideExtendedConverter()
        {
            quotes = true;
        }

        public OrderSideExtendedConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<OrderSideExtended, string> values = new Dictionary<OrderSideExtended, string>()
        {
            { OrderSideExtended.LimitBuy, "LIMIT_BUY" },
            { OrderSideExtended.LimitSell, "LIMIT_SELL" }
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(OrderSideExtended)value]);
            else
                writer.WriteRawValue(values[(OrderSideExtended)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderSideExtended);
        }
    }
}
