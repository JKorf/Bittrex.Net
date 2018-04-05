using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Bittrex.Net.Objects
{
    public class BittrexStreamBalanceData
    {
        [JsonProperty("N")]
        public int Nonce { get; set; }
        [JsonProperty("D")]
        public BittrexStreamBalance Delta { get; set; }
    }

    public class BittrexStreamBalance
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("U")]
        public Guid Guid { get; set; }
        /// <summary>
        /// Account id
        /// </summary>
        [JsonProperty("W")]
        public int AccountId { get; set; }
        /// <summary>
        /// Currency the balance is for
        /// </summary>
        [JsonProperty("c")]
        public string Currency { get; set; }
        /// <summary>
        /// The total balance
        /// </summary>
        [JsonProperty("b")]
        public decimal Balance { get; set; }
        /// <summary>
        /// The available balance
        /// </summary>
        [JsonProperty("a")]
        public decimal Available { get; set; }
        /// <summary>
        /// The pending balance
        /// </summary>
        [JsonProperty("z")]
        public decimal Pending { get; set; }
        /// <summary>
        /// The deposit address
        /// </summary>
        [JsonProperty("P")]
        public string CryptoAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("r")]
        public bool Requested { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonProperty("u"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Updated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("h")]
        public bool? AutoSell { get; set; }
    }
}
