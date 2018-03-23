using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class FillTypeConverter: BaseConverter<FillType>
    {
        public FillTypeConverter(): this(true) { }
        public FillTypeConverter(bool quotes) : base(quotes){ }

        protected override Dictionary<FillType, string> Mapping => new Dictionary<FillType, string>
        {
            { FillType.Fill, "FILL" },
            { FillType.PartialFill, "PARTIAL_FILL" }
        };
    }
}
