namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Deposit update
    /// </summary>
    public class BittrexDepositUpdate
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
        public BittrexDeposit Delta { get; set; } = default!;
    }
}
