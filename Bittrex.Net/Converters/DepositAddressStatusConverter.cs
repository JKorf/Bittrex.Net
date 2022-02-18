using System.Collections.Generic;
using Bittrex.Net.Enums;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class DepositAddressStatusConverter : BaseConverter<DepositAddressStatus>
    {
        public DepositAddressStatusConverter() : this(true) { }
        public DepositAddressStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<DepositAddressStatus, string>> Mapping => new List<KeyValuePair<DepositAddressStatus, string>>
        {
            new KeyValuePair<DepositAddressStatus, string>(DepositAddressStatus.Requested, "REQUESTED"),
            new KeyValuePair<DepositAddressStatus, string>(DepositAddressStatus.Provisioned, "PROVISIONED")
        };
    }
}
