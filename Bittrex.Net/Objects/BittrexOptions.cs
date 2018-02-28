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
        /// The address used to get CloudFlare clearance
        /// </summary>
        public string CloudFlareAuthenticationAddress { get; set; } = "https://www.bittrex.com/";
        /// <summary>
        /// The base address of the socket connection
        /// </summary>
        public string SocketAddress { get; set; } = "https://socket.bittrex.com/";
    }
}
