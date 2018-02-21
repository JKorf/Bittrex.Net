using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Converters;
using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamFill
    {
        /// <summary>
        /// Timestamp of the fill
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity of the fill
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Rate of the fill
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide OrderType { get; set; }
    }
}
