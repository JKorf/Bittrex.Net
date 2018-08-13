using System;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

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

        public override string AddAuthenticationToUriString(string uri, bool signed)
        {
            if (!signed)
                return uri;

            if (!uri.Contains("?"))
                uri += "?";

            if (!uri.EndsWith("?"))
                uri += "&";

            uri += $"apiKey={Credentials.Key.GetString()}&nonce={nonce}";
            return uri;
        }

        public override IRequest AddAuthenticationToRequest(IRequest request, bool signed)
        {
            if (!signed)
                return request;

            lock(locker)
                request.Headers.Add("apisign",
                    ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(request.Uri.ToString()))));

            return request;
        }

        public override string Sign(string toSign)
        {
            lock(locker)
                return BitConverter.ToString(encryptor.ComputeHash(Encoding.ASCII.GetBytes(toSign))).Replace("-", string.Empty);
        }
    }
}
