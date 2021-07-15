using System.Collections.Generic;
using Bittrex.Net.Objects;

namespace Bittrex.Net.Sockets
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
        public IEnumerable<BittrexExecution> Deltas { get; set; } = default!;
    }
}
