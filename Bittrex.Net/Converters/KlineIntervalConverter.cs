using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class KlineIntervalConverter : BaseConverter<KlineInterval>
    {
        public KlineIntervalConverter() : this(true) { }
        public KlineIntervalConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<KlineInterval, string>> Mapping => new List<KeyValuePair<KlineInterval, string>>
        {
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneMinute, "MINUTE_1"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.FiveMinutes, "MINUTE_5"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneHour, "HOUR_1"),
            new KeyValuePair<KlineInterval, string>(KlineInterval.OneDay, "DAY_1")
        };
    }
}
