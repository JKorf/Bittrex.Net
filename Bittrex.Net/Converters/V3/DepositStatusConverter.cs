using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters.V3
{
    public class DepositStatusConverter : BaseConverter<DepositStatus>
    {
        public DepositStatusConverter() : this(true) { }
        public DepositStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<DepositStatus, string>> Mapping => new List<KeyValuePair<DepositStatus, string>>
        {
            new KeyValuePair<DepositStatus, string>(DepositStatus.Pending, "PENDING"),
            new KeyValuePair<DepositStatus, string>(DepositStatus.Completed, "COMPLETED"),
            new KeyValuePair<DepositStatus, string>(DepositStatus.Orphaned, "ORPHANED"),
            new KeyValuePair<DepositStatus, string>(DepositStatus.Invalidated, "INVALIDATED"),
        };
    }
}
