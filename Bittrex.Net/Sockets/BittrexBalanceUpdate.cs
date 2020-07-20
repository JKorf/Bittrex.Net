using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Balance update
    /// </summary>
    public class BittrexBalanceUpdate
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
        public BittrexBalanceV3 Delta { get; set; }
    }
}
