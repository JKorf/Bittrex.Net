using CryptoExchange.Net.Attributes;

namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Transfer type
    /// </summary>
    public enum TransferType
    {
        /// <summary>
        /// Wire transfer
        /// </summary>
        [Map("WIRE")]
        Wire,
        /// <summary>
        /// Sepa transfer
        /// </summary>
        [Map("SEPA")]
        Sepa,
        /// <summary>
        /// Instant settlement
        /// </summary>
        [Map("INSTANT_SETTLEMENT")]
        InstantSettlement,
        /// <summary>
        /// Ach
        /// </summary>
        [Map("ACH")]
        Ach,
        /// <summary>
        /// Sen
        /// </summary>
        [Map("SEN")]
        Sen
    }
}
