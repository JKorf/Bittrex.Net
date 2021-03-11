using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;

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
        public string AccountId { get; set; } = "";
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
