using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters.V3
{
    public class TimeInForceConverter : BaseConverter<TimeInForce>
    {
        public TimeInForceConverter() : this(true) { }
        public TimeInForceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TimeInForce, string>> Mapping => new List<KeyValuePair<TimeInForce, string>>
        {
            new KeyValuePair<TimeInForce, string>(TimeInForce.GoodTillCancelled, "GOOD_TIL_CANCELLED"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.ImmediateOrCancel, "IMMEDIATE_OR_CANCEL"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.FillOrKill, "FILL_OR_KILL"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.PostOnlyGoodTillCancelled, "POST_ONLY_GOOD_TIL_CANCELLED"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.PostOnlyGoodTillDate, "POST_ONLY_GOOD_TIL_DATE"),
        };
    }
}
