using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters.V3
{
    internal class BittrexConditionalOrderOperandConverter : BaseConverter<BittrexConditionalOrderOperand>
    {
        public BittrexConditionalOrderOperandConverter() : this(true) { }
        public BittrexConditionalOrderOperandConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BittrexConditionalOrderOperand, string>> Mapping => new List<KeyValuePair<BittrexConditionalOrderOperand, string>>
        {
            new KeyValuePair<BittrexConditionalOrderOperand, string>(BittrexConditionalOrderOperand.GreaterThan, "GTE"),
            new KeyValuePair<BittrexConditionalOrderOperand, string>(BittrexConditionalOrderOperand.LesserThan, "LTE")
        };
    }
}
