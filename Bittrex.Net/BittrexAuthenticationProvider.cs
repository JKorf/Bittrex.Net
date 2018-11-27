using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;

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
}
