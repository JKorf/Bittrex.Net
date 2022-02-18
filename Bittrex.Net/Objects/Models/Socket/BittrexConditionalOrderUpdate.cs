namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Deposit update
    /// </summary>
    public class BittrexConditionalOrderUpdate
    {
        /// <summary>
        /// Account id for the deposit
        /// </summary>
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// Update data
        /// </summary>
        public BittrexConditionalOrder Delta { get; set; } = default!;
    }
}
