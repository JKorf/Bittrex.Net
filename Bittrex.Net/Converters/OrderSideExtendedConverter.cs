using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    public class OrderSideExtendedConverter : BaseConverter<OrderSideExtended>
    {
        public OrderSideExtendedConverter(): this(true) { }
        public OrderSideExtendedConverter(bool quotes) : base(quotes){ }

        protected override Dictionary<OrderSideExtended, string> Mapping => new Dictionary<OrderSideExtended, string>
        {
            { OrderSideExtended.LimitBuy, "LIMIT_BUY" },
            { OrderSideExtended.LimitSell, "LIMIT_SELL" }
        };
    }
}
