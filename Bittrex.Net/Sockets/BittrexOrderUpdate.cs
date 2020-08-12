using System;
using System.Collections.Generic;
using System.Text;
using Bittrex.Net.Objects.V3;
using Newtonsoft.Json;

namespace Bittrex.Net.Sockets
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
        public string AccountId { get; set; } = "";

        /// <summary>
        /// Changed order
        /// </summary>
        public BittrexOrderV3 Delta { get; set; } = default!;
    }
}
