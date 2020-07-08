using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Account info
    /// </summary>
    public class BittrexAccount
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string AccountId { get; set; } = "";
        /// <summary>
        /// Sub account id
        /// </summary>
        public string SubAccountId { get; set; } = "";
    }
}
