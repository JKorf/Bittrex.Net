using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;

namespace Bittrex.Net.Sockets
{
    /// <summary>
    /// Deposit update
    /// </summary>
    public class BittrexDepositUpdate
    {
        /// <summary>
        /// Account id for the deposit
        /// </summary>
        public string AccountId { get; set; } = "";
        /// <summary>
        /// Sequence
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// Update data
        /// </summary>
        public BittrexDepositV3 Delta { get; set; } = default!;
    }
}
