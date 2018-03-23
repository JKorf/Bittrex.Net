using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class ConditionTypeConverter: BaseConverter<ConditionType>
    {
        public ConditionTypeConverter(): this(true) { }
        public ConditionTypeConverter(bool quotes) : base(quotes) { }

        protected override Dictionary<ConditionType, string> Mapping => new Dictionary<ConditionType, string>
        {
            { ConditionType.None, "NONE" },
            { ConditionType.GreaterThan, "GREATER_THAN" },
            { ConditionType.LessThan, "LESS_THAN" },
            { ConditionType.StopLossFixed, "STOP_LOSS_FIXED" },
            { ConditionType.StopLossPercentage, "STOP_LOSS_PERCENTAGE" }
        };
    }
}
