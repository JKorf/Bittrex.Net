using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class FillTypeConverter: JsonConverter
    {
        private readonly bool quotes;

        public FillTypeConverter()
        {
            quotes = true;
        }

        public FillTypeConverter(bool useQuotes)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<FillType, string> values = new Dictionary<FillType, string>()
        {
            { FillType.Fill, "FILL" },
            { FillType.PartialFill, "PARTIAL_FILL" }
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(FillType)value]);
            else
                writer.WriteRawValue(values[(FillType)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FillType);
        }
    }
}
