using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "LIMIT"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "MARKET"),
            new KeyValuePair<OrderType, string>(OrderType.Limit, "CEILING_LIMIT"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "CEILING_MARKET"),
        };
    }
}
