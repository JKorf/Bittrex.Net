using System.Collections.Generic;

namespace Bittrex.Net.Objects.Models.Socket
{
    /// <summary>
    /// Execution update
    /// </summary>
    public class BittrexExecutionUpdate
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
        public IEnumerable<BittrexUserTrade> Deltas { get; set; } = default!;
    }
}
