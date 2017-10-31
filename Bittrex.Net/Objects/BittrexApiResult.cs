using Newtonsoft.Json;

namespace Bittrex.Net.Objects
{
    public class BittrexApiResult<T>
    {
        [JsonProperty("success")]
        public bool Success { get; internal set; }
        [JsonProperty("result")]
        public T Result { get; internal set; }
        [JsonProperty("message")]
        public string Message { get; internal set; }
    }
}
