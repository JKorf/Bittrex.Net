using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net
{
    public class BittrexAuthenticationProvider: AuthenticationProvider
    {
        private static long nonce => DateTime.UtcNow.Ticks;
        private readonly HMACSHA512 encryptor;
        private readonly object locker;

        public BittrexAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            locker = new object();
            encryptor = new HMACSHA512(Encoding.ASCII.GetBytes(credentials.Secret.GetString()));
        }

        public override Dictionary<string, object> AddAuthenticationToParameters(string uri, string method, Dictionary<string, object> parameters, bool signed)
        {
            if (!signed)
                return parameters;

            lock(locker)
                parameters.Add("apiKey", Credentials.Key.GetString());
            parameters.Add("nonce", nonce);
            return parameters;
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, string method, Dictionary<string, object> parameters, bool signed)
        {
            if (!signed)
                return new Dictionary<string, string>();

            var result = new Dictionary<string, string>();
            lock (locker)
                result.Add("apisign", ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(uri))));
            return result;
        }

        public override string Sign(string toSign)
        {
            lock(locker)
                return BitConverter.ToString(encryptor.ComputeHash(Encoding.ASCII.GetBytes(toSign))).Replace("-", string.Empty);
        }
    }

    public class BittrexAuthenticationProviderV3 : AuthenticationProvider
    {
        private readonly HMACSHA512 encryptorHmac;
        private readonly SHA512 encryptor;
        private readonly object locker;

        public BittrexAuthenticationProviderV3(ApiCredentials credentials) : base(credentials)
        {
            locker = new object();
            encryptorHmac = new HMACSHA512(Encoding.ASCII.GetBytes(credentials.Secret.GetString()));
            encryptor = SHA512.Create();
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, string method, Dictionary<string, object> parameters, bool signed)
        {
            if (!signed)
                return new Dictionary<string, string>();

            var result = new Dictionary<string, string>();
            lock (locker)
                result.Add("Api-Key", Credentials.Key.GetString());
            result.Add("Api-Timestamp", Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(CultureInfo.InvariantCulture));
            string jsonContent = "";
            if(method != Constants.GetMethod && method != Constants.DeleteMethod)
                jsonContent = JsonConvert.SerializeObject(parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));
            result.Add("Api-Content-Hash", ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(jsonContent))).ToLower());

            uri = WebUtility.UrlDecode(uri); // Sign needs the query parameters to not be encoded
            var sign = result["Api-Timestamp"] + uri + method + result["Api-Content-Hash"] + "";
            result.Add("Api-Signature", ByteToString(encryptorHmac.ComputeHash(Encoding.UTF8.GetBytes(sign))));
            return result;
        }

        public override string Sign(string toSign)
        {
            lock (locker)
                return BitConverter.ToString(encryptor.ComputeHash(Encoding.ASCII.GetBytes(toSign))).Replace("-", string.Empty);
        }
    }
}
