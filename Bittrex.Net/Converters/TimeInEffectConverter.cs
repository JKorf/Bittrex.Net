using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class TimeInEffectConverter: JsonConverter
    {
        private readonly bool quotes;

        public TimeInEffectConverter()
        {
            quotes = true;
        }

        public TimeInEffectConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<TimeInEffect, string> values = new Dictionary<TimeInEffect, string>()
        {
            { TimeInEffect.GoodTillCancelled, "GOOD_TIL_CANCELLED" },
            { TimeInEffect.ImmediateOrCancel, "IMMEDIATE_OR_CANCEL" },
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(TimeInEffect)value]);
            else
                writer.WriteRawValue(values[(TimeInEffect)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value.ToLower() == reader.Value.ToString().ToLower()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeInEffect);
        }
    }
}
