using System.Collections.Generic;

namespace Bittrex.Net.Objects.Models
{
    /// <summary>
    /// Error info
    /// </summary>
    public class BittrexError
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Error details
        /// </summary>
        public string Detail { get; set; } = string.Empty;
        /// <summary>
        /// Additional data for the error
        /// </summary>
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}
