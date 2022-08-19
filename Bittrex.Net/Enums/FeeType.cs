using CryptoExchange.Net.Attributes;

namespace Bittrex.Net.Enums
{
    /// <summary>
    /// Fee type
    /// </summary>
    public enum FeeType
    {
        /// <summary>
        /// Fixed
        /// </summary>
        [Map("FIXED")]
        Fixed,
        /// <summary>
        /// Percent
        /// </summary>
        [Map("PERCENT")]
        Percent
    }
}
