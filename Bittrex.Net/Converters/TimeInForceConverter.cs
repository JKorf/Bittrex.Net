using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class TimeInForceConverter : BaseConverter<TimeInForce>
    {
        public TimeInForceConverter() : this(true) { }
        public TimeInForceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TimeInForce, string>> Mapping => new List<KeyValuePair<TimeInForce, string>>
        {
            new KeyValuePair<TimeInForce, string>(TimeInForce.GoodTillCanceled, "GOOD_TIL_CANCELLED"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.ImmediateOrCancel, "IMMEDIATE_OR_CANCEL"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.FillOrKill, "FILL_OR_KILL"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.PostOnlyGoodTillCanceled, "POST_ONLY_GOOD_TIL_CANCELLED"),
            new KeyValuePair<TimeInForce, string>(TimeInForce.PostOnlyGoodTillDate, "POST_ONLY_GOOD_TIL_DATE"),
        };
    }
}
