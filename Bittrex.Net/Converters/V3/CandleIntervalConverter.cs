using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters.V3
{
    internal class CandleIntervalConverter : BaseConverter<CandleInterval>
    {
        public CandleIntervalConverter() : this(true) { }
        public CandleIntervalConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<CandleInterval, string>> Mapping => new List<KeyValuePair<CandleInterval, string>>
        {
            new KeyValuePair<CandleInterval, string>(CandleInterval.OneMinute, "MINUTE_1"),
            new KeyValuePair<CandleInterval, string>(CandleInterval.FiveMinutes, "MINUTE_5"),
            new KeyValuePair<CandleInterval, string>(CandleInterval.OneHour, "HOUR_1"),
            new KeyValuePair<CandleInterval, string>(CandleInterval.OneDay, "DAY_1")
        };
    }
}
