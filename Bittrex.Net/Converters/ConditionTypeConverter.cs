using System;
using System.Collections.Generic;
using System.Linq;
using Bittrex.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net.Converters
{
    public class ConditionTypeConverter: JsonConverter
    {
        private readonly bool quotes;

        public ConditionTypeConverter()
        {
            quotes = true;
        }

        public ConditionTypeConverter(bool useQuotes = true)
        {
            quotes = useQuotes;
        }

        private readonly Dictionary<ConditionType, string> values = new Dictionary<ConditionType, string>()
        {
            { ConditionType.None, "NONE" },
            { ConditionType.GreaterThan, "GREATER_THAN" },
            { ConditionType.LessThan, "LESS_THAN" },
            { ConditionType.StopLossFixed, "STOP_LOSS_FIXED" },
            { ConditionType.StopLossPercentage, "STOP_LOSS_PERCENTAGE" },
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (quotes)
                writer.WriteValue(values[(ConditionType)value]);
            else
                writer.WriteRawValue(values[(ConditionType)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return values.Single(v => v.Value.ToLower() == reader.Value.ToString().ToLower()).Key;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ConditionType);
        }
    }
}
