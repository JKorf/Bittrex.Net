using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class SymbolStatusConverter : BaseConverter<SymbolStatus>
    {
        public SymbolStatusConverter() : this(true) { }
        public SymbolStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<SymbolStatus, string>> Mapping => new List<KeyValuePair<SymbolStatus, string>>
        {
            new KeyValuePair<SymbolStatus, string>(SymbolStatus.Online, "ONLINE"),
            new KeyValuePair<SymbolStatus, string>(SymbolStatus.Offline, "OFFLINE"),
        };
    }
}
