using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class ConditionalOrderOperandConverter : BaseConverter<ConditionalOrderOperand>
    {
        public ConditionalOrderOperandConverter() : this(true) { }
        public ConditionalOrderOperandConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<ConditionalOrderOperand, string>> Mapping => new List<KeyValuePair<ConditionalOrderOperand, string>>
        {
            new KeyValuePair<ConditionalOrderOperand, string>(ConditionalOrderOperand.GreaterThan, "GTE"),
            new KeyValuePair<ConditionalOrderOperand, string>(ConditionalOrderOperand.LesserThan, "LTE")
        };
    }
}
