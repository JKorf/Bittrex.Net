namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Order update
    /// </summary>
    public class BittrexOrderUpdate
    {
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Account id
        /// </summary>
        public string AccountId { get; set; } = string.Empty;

        /// <summary>
        /// Changed order
        /// </summary>
        public BittrexOrder Delta { get; set; } = default!;
    }
}
