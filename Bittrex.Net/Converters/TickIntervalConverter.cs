using Bittrex.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bittrex.Net.Converters
{
    public class TickIntervalConverter: JsonConverter
    {
        private readonly bool quotes;

        public TickIntervalConverter()
        {
            quotes = true;
        }

        public TickIntervalConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<TickInterval, string> values = new Dictionary<TickInterval, string>()
        {
            { TickInterval.OneMinute, "oneMin" },
            { TickInterval.FiveMinutes, "fiveMin" },
            { TickInterval.HalfHour, "thirtyMin" },
            { TickInterval.OneHour, "hour" },
            { TickInterval.OneDay, "day" },
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(TickInterval)value]);
            else
                writer.WriteRawValue(values[(TickInterval)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value == reader.Value.ToString()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TickInterval);
        }
    }
}
