using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Bittrex.Net.Converters
{
    internal class OrderUpdateTypeConverter : BaseConverter<OrderUpdateType>
    {
        public OrderUpdateTypeConverter() :this(true)
        {
        }

        public OrderUpdateTypeConverter(bool useQuotes) : base(useQuotes)
        {
        }

        protected override List<KeyValuePair<OrderUpdateType, string>> Mapping => new List<KeyValuePair<OrderUpdateType, string>>
        {
            new KeyValuePair<OrderUpdateType, string>(OrderUpdateType.Open, "0"),
            new KeyValuePair<OrderUpdateType, string>(OrderUpdateType.PartialFill, "1"),
            new KeyValuePair<OrderUpdateType, string>(OrderUpdateType.Fill, "2"),
            new KeyValuePair<OrderUpdateType, string>(OrderUpdateType.Cancel, "3")
        };
    }
}
