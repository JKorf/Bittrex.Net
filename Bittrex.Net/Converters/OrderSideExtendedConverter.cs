using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class OrderSideExtendedConverter : BaseConverter<OrderSideExtended>
    {
        public OrderSideExtendedConverter(): this(true) { }
        public OrderSideExtendedConverter(bool quotes) : base(quotes){ }

        protected override List<KeyValuePair<OrderSideExtended, string>> Mapping => new List<KeyValuePair<OrderSideExtended, string>>
        {
            new KeyValuePair<OrderSideExtended, string>(OrderSideExtended.LimitBuy, "LIMIT_BUY"),
            new KeyValuePair<OrderSideExtended, string>(OrderSideExtended.LimitSell, "LIMIT_SELL"),
            new KeyValuePair<OrderSideExtended, string>(OrderSideExtended.LimitSell, "MARKET_SELL"),
            new KeyValuePair<OrderSideExtended, string>(OrderSideExtended.LimitSell, "MARKET_BUY")
        };
    }
}
