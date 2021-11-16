namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Symbol permission
    /// </summary>
    public class BittrexSymbolPermission
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Allowed to view
        /// </summary>
        public bool View { get; set; }
        /// <summary>
        /// Allowed to buy
        /// </summary>
        public bool Buy { get; set; }
        /// <summary>
        /// Allowed to sell
        /// </summary>
        public bool Sell { get; set; }
    }
}
