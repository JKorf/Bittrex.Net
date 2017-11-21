namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Information about a balance
    /// </summary>
    public class BittrexBalance
    {
        /// <summary>
        /// The currency for which the balance is
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The total balance
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// The available balance
        /// </summary>
        public decimal Available { get; set; }
        /// <summary>
        /// The pending balance
        /// </summary>
        public decimal Pending { get; set; }
        /// <summary>
        /// The crypto address this balance is on
        /// </summary>
        public string CryptoAddress { get; set; }
        /// <summary>
        /// Requested
        /// </summary>
        public bool Requested { get; set; }
        /// <summary>
        /// Guid
        /// </summary>
        public string Uuid { get; set; }
    }
}
