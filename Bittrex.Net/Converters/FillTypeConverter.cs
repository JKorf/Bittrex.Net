using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class FillTypeConverter: BaseConverter<FillType>
    {
        public FillTypeConverter(): this(true) { }
        public FillTypeConverter(bool quotes) : base(quotes){ }

        protected override List<KeyValuePair<FillType, string>> Mapping => new List<KeyValuePair<FillType, string>>
        {
            new KeyValuePair<FillType, string>(FillType.Fill, "FILL"),
            new KeyValuePair<FillType, string>(FillType.PartialFill, "PARTIAL_FILL")
        };
    }
}
