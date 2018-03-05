using CryptoExchange.Net;

namespace Bittrex.Net.Objects
{
    public class BittrexClientOptions: ExchangeOptions
    {
        /// <summary>
        /// The address to use as base for the api calls
        /// </summary>
        public string BaseAddress { get; set; } = "https://www.bittrex.com";
    }

    public class BittrexSocketClientOptions : ExchangeOptions
    {
        /// <summary>
        /// The max amount of retries to bypass the CloudFlare protection
        /// </summary>
        public int CloudFlareBypassRetries { get; set; } = 2;
        /// <summary>
        /// The address used to get CloudFlare clearance
        /// </summary>
        public string CloudFlareAuthenticationAddress { get; set; } = "https://www.bittrex.com/";
        /// <summary>
        /// The base address of the socket connection
        /// </summary>
        public string SocketAddress { get; set; } = "https://socket.bittrex.com/";
    }
}
