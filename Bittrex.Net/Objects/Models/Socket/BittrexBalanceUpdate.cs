namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Balance update
    /// </summary>
    public class BittrexBalanceUpdate
    {
        /// <summary>
        /// Account id for the balance change
        /// </summary>
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// Update data
        /// </summary>
        public BittrexBalance Delta { get; set; } = default!;
    }
}
