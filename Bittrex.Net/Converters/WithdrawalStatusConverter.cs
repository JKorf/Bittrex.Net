using System.Collections.Generic;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Converters;

namespace Bittrex.Net.Converters
{
    internal class WithdrawalStatusConverter : BaseConverter<WithdrawalStatus>
    {
        public WithdrawalStatusConverter() : this(true) { }
        public WithdrawalStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<WithdrawalStatus, string>> Mapping => new List<KeyValuePair<WithdrawalStatus, string>>
        {
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.Requested, "REQUESTED"),
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.Authorized, "AUTHORIZED"),
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.Pending, "PENDING"),
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.Completed, "COMPLETED"),
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.InvalidAddress, "ERROR_INVALID_ADDRESS"),
            new KeyValuePair<WithdrawalStatus, string>(WithdrawalStatus.Cancelled, "CANCELLED"),
        };
    }
}
