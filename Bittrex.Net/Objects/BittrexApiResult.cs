using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// The result of an Api call
    /// </summary>
    /// <typeparam name="T">The result type</typeparam>
    public class BittrexApiResult<T>
    {
        /// <summary>
        /// Whether the Api call was successful
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; internal set; }
        /// <summary>
        /// The result of the Api call
        /// </summary>
        [JsonProperty("result")]
        public T Result { get; internal set; }

        [JsonProperty("message")]
        internal string Message { get; set; }

        /// <summary>
        /// The error if the call wasn't successful
        /// </summary>
        [JsonIgnore]
        public BittrexError Error { get; internal set; }
    }
}
