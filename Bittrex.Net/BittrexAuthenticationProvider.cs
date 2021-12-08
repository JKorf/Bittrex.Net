using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net
{
    internal class BittrexAuthenticationProvider : AuthenticationProvider
    {
        public BittrexAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateBodyRequest(RestApiClient apiClient, Uri uri, HttpMethod method, SortedDictionary<string, object> parameters, Dictionary<string, string> headers, bool auth, ArrayParametersSerialization arraySerialization)
        {
            if (!auth)
                return;

            headers.Add("Api-Key", Credentials.Key!.GetString());
            headers.Add("Api-Timestamp", GetTimestamp(apiClient));

            var jsonContent = string.Empty;
            if (parameters.Any() && parameters.First().Key == string.Empty)
                jsonContent = JsonConvert.SerializeObject(parameters.First().Value);
            else
                jsonContent = JsonConvert.SerializeObject(parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));
            headers.Add("Api-Content-Hash", SignSHA512(jsonContent));

            var uriString = WebUtility.UrlDecode(uri.ToString()); // Sign needs the query parameters to not be encoded
            headers.Add("Api-Signature", SignHMACSHA512(headers["Api-Timestamp"] + uriString + method + headers["Api-Content-Hash"]));
        }

        public override void AuthenticateUriRequest(RestApiClient apiClient, Uri uri, HttpMethod method, SortedDictionary<string, object> parameters, Dictionary<string, string> headers, bool auth, ArrayParametersSerialization arraySerialization)
        {
            if (!auth)
                return;

            headers.Add("Api-Key", Credentials.Key!.GetString());
            headers.Add("Api-Timestamp", Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(CultureInfo.InvariantCulture));
            headers.Add("Api-Content-Hash", SignSHA512(string.Empty));

            var uriString = WebUtility.UrlDecode(uri.ToString()); // Sign needs the query parameters to not be encoded
            headers.Add("Api-Signature", SignHMACSHA512(headers["Api-Timestamp"] + uriString + method + headers["Api-Content-Hash"]));
        }

        public override string Sign(string toSign) => SignHMACSHA512(toSign);

        internal string GetTimestamp(RestApiClient apiClient)
        {
            return DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow.Add(apiClient.GetTimeOffset()))!.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
