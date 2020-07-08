using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.V3
{
    /// <summary>
    /// Volume info
    /// </summary>
    public class BittrexAccountVolume
    {
        /// <summary>
        /// Update time of the volume
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// Volume 30 days
        /// </summary>
        public decimal Volume30Days { get; set; }
    }
}
