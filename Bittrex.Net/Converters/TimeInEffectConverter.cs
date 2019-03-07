using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class TimeInEffectConverter: BaseConverter<TimeInEffect>
    {
        public TimeInEffectConverter(): this(true) { }
        public TimeInEffectConverter(bool quotes) : base(quotes){ }

        protected override List<KeyValuePair<TimeInEffect, string>> Mapping => new List<KeyValuePair<TimeInEffect, string>>
        {
            new KeyValuePair<TimeInEffect, string>(TimeInEffect.GoodTillCancelled, "GOOD_TIL_CANCELLED"),
            new KeyValuePair<TimeInEffect, string>(TimeInEffect.ImmediateOrCancel, "IMMEDIATE_OR_CANCEL")
        };
    }
}
