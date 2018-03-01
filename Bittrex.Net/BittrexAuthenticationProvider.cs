using System;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net
{
    public class BittrexAuthenticationProvider: AuthenticationProvider
    {
        private long nonce => DateTime.UtcNow.Ticks;
        private HMACSHA512 encryptor;

        public BittrexAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            encryptor = new HMACSHA512(Encoding.ASCII.GetBytes(credentials.Secret));
        }

        public override string AddAuthenticationToUriString(string uri, bool signed)
        {
            if (!signed)
                return uri;

            if (!uri.EndsWith("?"))
                uri += "&";

            uri += $"apiKey={credentials.Key}&nonce={nonce}";
            return uri;
        }

        public override IRequest AddAuthenticationToRequest(IRequest request, bool signed)
        {
            if (!signed)
                return request;

            request.Headers.Add("apisign",
                ByteToString(encryptor.ComputeHash(Encoding.UTF8.GetBytes(request.Uri.ToString()))));
            return request;
        }
    }
}
