namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a deposit address for a currency
    /// </summary>
    public class BittrexDepositAddress
    {
        /// <summary>
        /// Currency the address is for
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The deposit address of the currency
        /// </summary>
        public string Address { get; set; }
    }
}
