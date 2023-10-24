using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace Bittrex.Net
{
    internal class BittrexAuthenticationProvider : AuthenticationProvider
    {
        public string GetApiKey() => _credentials.Key!.GetString();

        public BittrexAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            if (credentials.CredentialType != ApiCredentialsType.Hmac)
                throw new Exception("Only Hmac authentication is supported");
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            if (!auth)
                return;

            headers.Add("Api-Key", _credentials.Key!.GetString());
            headers.Add("Api-Timestamp", GetMillisecondTimestamp(apiClient));

            string jsonContent = string.Empty;
            if (parameterPosition == HttpMethodParameterPosition.InBody)
            {
                if (bodyParameters.Any() && bodyParameters.First().Key == string.Empty)
                    jsonContent = JsonConvert.SerializeObject(bodyParameters.First().Value);
                else
                    jsonContent = JsonConvert.SerializeObject(bodyParameters);
            }
            headers.Add("Api-Content-Hash", SignSHA512(jsonContent));

            uri = uri.SetParameters(uriParameters, arraySerialization);
            var uriString = WebUtility.UrlDecode(uri.ToString()); // Sign needs the query parameters to not be encoded
            headers.Add("Api-Signature", SignHMACSHA512(headers["Api-Timestamp"] + uriString + method + headers["Api-Content-Hash"]));
        }

        public string Sign(string toSign) => SignHMACSHA512(toSign);
    }
}
