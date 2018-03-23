using Bittrex.Net.Objects;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class TickIntervalConverter: BaseConverter<TickInterval>
    {
        public TickIntervalConverter(): this(true) { }
        public TickIntervalConverter(bool quotes) : base(quotes){ }

        protected override Dictionary<TickInterval, string> Mapping => new Dictionary<TickInterval, string>
        {
            { TickInterval.OneMinute, "oneMin" },
            { TickInterval.FiveMinutes, "fiveMin" },
            { TickInterval.HalfHour, "thirtyMin" },
            { TickInterval.OneHour, "hour" },
            { TickInterval.OneDay, "day" }
        };
    }
}
