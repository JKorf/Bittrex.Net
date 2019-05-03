using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters.V3
{
    public class OrderTypeConverter : BaseConverter<OrderTypeV3>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderTypeV3, string>> Mapping => new List<KeyValuePair<OrderTypeV3, string>>
        {
            new KeyValuePair<OrderTypeV3, string>(OrderTypeV3.Limit, "LIMIT"),
            new KeyValuePair<OrderTypeV3, string>(OrderTypeV3.Market, "MARKET"),
            new KeyValuePair<OrderTypeV3, string>(OrderTypeV3.CeilingLimit, "CEILING_LIMIT"),
            new KeyValuePair<OrderTypeV3, string>(OrderTypeV3.CeilingMarket, "CEILING_MARKET"),
        };
    }
}
