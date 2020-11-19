using System.Net.Http;
using Bittrex.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Options for the Bittrex client
    /// </summary>
    public class BittrexClientOptions : RestClientOptions
    {
        /// <summary>
        /// Create new client options
        /// </summary>
        public BittrexClientOptions() : base("https://api.bittrex.com")
        {
        }

        /// <summary>
        /// Create new client options
        /// </summary>
        /// <param name="client">HttpClient to use for requests from this client</param>
        public BittrexClientOptions(HttpClient client) : base(client, "https://api.bittrex.com")
        {
        }

        /// <summary>
        /// Create new client options
        /// </summary>
        /// <param name="apiAddress">Custom API address to use</param>
        /// <param name="client">HttpClient to use for requests from this client</param>
        public BittrexClientOptions(HttpClient client, string apiAddress) : base(client, apiAddress)
        {
        }

        /// <summary>
        /// The V2 API base address
        /// </summary>
        public string BaseAddressV2 { get; set; } = "https://international.bittrex.com";

        /// <summary>
        /// Copy the options
        /// </summary>
        /// <returns></returns>
        public BittrexClientOptions Copy()
        {
            var copy = Copy<BittrexClientOptions>();
            copy.BaseAddressV2 = BaseAddressV2;
            return copy;
        }
    }

    /// <summary>
    /// Options for the Bittrex socket client
    /// </summary>
    public class BittrexSocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BittrexSocketClientOptions(): base("https://socket.bittrex.com")
        {
            SocketSubscriptionsCombineTarget = 10;
        }
    }

    /// <summary>
    /// Options for the Bittrex socket client
    /// </summary>
    public class BittrexSocketClientV3Options : SocketClientOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BittrexSocketClientV3Options() : base("https://socket-v3.bittrex.com")
        {
            SocketSubscriptionsCombineTarget = 10;
        }
    }

    /// <summary>
    /// Options for the Bittrex symbol order book
    /// </summary>
    public class BittrexOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IBittrexSocketClientV3? SocketClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="socketClient">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        public BittrexOrderBookOptions(IBittrexSocketClientV3? socketClient = null) : base("Bittrex", true, true)
        {
            SocketClient = socketClient;
        }
    }
}
