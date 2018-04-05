using System;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net
{
    public class BittrexAuthenticationProvider: AuthenticationProvider
    {
        private static long nonce => DateTime.UtcNow.Ticks;
        private readonly HMACSHA512 encryptor;

        public BittrexAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            encryptor = new HMACSHA512(Encoding.ASCII.GetBytes(credentials.Secret));
        }

        public override string AddAuthenticationToUriString(string uri, bool signed)
        {
            if (!signed)
                return uri;

            if (!uri.Contains("?"))
                uri += "?";

            if (!uri.EndsWith("?"))
                uri += "&";

            uri += $"apiKey={Credentials.Key}&nonce={nonce}";
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

        public override string Sign(string toSign)
        {
            return BitConverter.ToString(encryptor.ComputeHash(Encoding.ASCII.GetBytes(toSign))).Replace("-", string.Empty);
        }
    }
}
