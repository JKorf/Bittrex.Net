using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Bittrex.Net.Converters
{
    public class OrderUpdateTypeConverter : BaseConverter<OrderUpdateType>
    {
        public OrderUpdateTypeConverter() :this(true)
        {
        }

        public OrderUpdateTypeConverter(bool useQuotes) : base(useQuotes)
        {
        }

        protected override Dictionary<OrderUpdateType, string> Mapping => new Dictionary<OrderUpdateType, string>
        {
            { OrderUpdateType.Open, "0" },
            { OrderUpdateType.PartialFill, "1" },
            { OrderUpdateType.Fill, "2" },
            { OrderUpdateType.Cancel, "3" },
        };
    }
}
