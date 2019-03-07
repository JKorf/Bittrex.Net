using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class ConditionTypeConverter: BaseConverter<ConditionType>
    {
        public ConditionTypeConverter(): this(true) { }
        public ConditionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<ConditionType, string>> Mapping => new List<KeyValuePair<ConditionType, string>>
        {
            new KeyValuePair<ConditionType, string>(ConditionType.None, ""),
            new KeyValuePair<ConditionType, string>(ConditionType.None, "NONE"),
            new KeyValuePair<ConditionType, string>(ConditionType.GreaterThan, "GREATER_THAN"),
            new KeyValuePair<ConditionType, string>(ConditionType.LessThan, "LESS_THAN"),
            new KeyValuePair<ConditionType, string>(ConditionType.StopLossFixed, "STOP_LOSS_FIXED"),
            new KeyValuePair<ConditionType, string>(ConditionType.StopLossPercentage, "STOP_LOSS_PERCENTAGE"),
        };
    }
}
