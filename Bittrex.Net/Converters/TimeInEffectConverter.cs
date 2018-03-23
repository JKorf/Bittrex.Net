using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class TimeInEffectConverter: BaseConverter<TimeInEffect>
    {
        public TimeInEffectConverter(): this(true) { }
        public TimeInEffectConverter(bool quotes) : base(quotes){ }

        protected override Dictionary<TimeInEffect, string> Mapping => new Dictionary<TimeInEffect, string>
        {
            { TimeInEffect.GoodTillCancelled, "GOOD_TIL_CANCELLED" },
            { TimeInEffect.ImmediateOrCancel, "IMMEDIATE_OR_CANCEL" }
        };
    }
}
