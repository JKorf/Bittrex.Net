using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bittrex.Net.Converters
{
    internal class BatchResultConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var result = new List<CallResult<T>>();
            if (reader.TokenType != JsonToken.StartArray)
            {
                Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Failed to deserialize batch result. Data: " + result);
                return default;
            }

            var array = JArray.Load(reader);

            foreach(JObject item in array)
            {
                var statusToken = item["status"];
                if(statusToken == null || statusToken.Type == JTokenType.Null)
                {
                    Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Failed to deserialize batch result, no status property. Data: " + result);
                    return default;
                }

                var status = statusToken.Value<int>();
                if (status == 200)
                {
                    var data = item["payload"];
                    if (data == null || data.Type == JTokenType.Null)
                    {
                        Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Failed to deserialize batch result, no payload property. Data: " + result);
                        return default;
                    }

                    var converted = (T)data.ToObject(typeof(T));
                    result.Add(new CallResult<T>(converted!));
                }
                else
                {
                    var error = item["payload"];
                    if (error == null)
                        result.Add(new CallResult<T>(new UnknownError("Unknown payload structure")));
                    else
                    {
                        var msg = error["code"]?.ToString();
                        result.Add(new CallResult<T>(new ServerError(status, msg ?? "Unknown error")));
                    }
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
