using Bittrex.Net.Objects;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class TickIntervalConverter: BaseConverter<TickInterval>
    {
        public TickIntervalConverter(): this(true) { }
        public TickIntervalConverter(bool quotes) : base(quotes){ }

        protected override List<KeyValuePair<TickInterval, string>> Mapping => new List<KeyValuePair<TickInterval, string>>
        {
            new KeyValuePair<TickInterval, string>(TickInterval.OneMinute, "oneMin"),
            new KeyValuePair<TickInterval, string>(TickInterval.FiveMinutes, "fiveMin"),
            new KeyValuePair<TickInterval, string>(TickInterval.HalfHour, "thirtyMin"),
            new KeyValuePair<TickInterval, string>(TickInterval.OneHour, "hour"),
            new KeyValuePair<TickInterval, string>(TickInterval.OneDay, "day")
        };
    }
}
