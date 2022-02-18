using System;
using System.Collections.Generic;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Account info
    /// </summary>
    public class BittrexAccount
    {
        /// <summary>
        /// Account id
        /// </summary>
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Sub account id
        /// </summary>
        public string SubAccountId { get; set; } = string.Empty;

        /// <summary>
        /// Actions needed
        /// </summary>
        public IEnumerable<string> ActionsNeeded { get; set; } = Array.Empty<string>();
    }
}
