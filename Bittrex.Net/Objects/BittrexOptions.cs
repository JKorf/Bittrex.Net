using System.Net.Http;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Interfaces.Clients.Rest;
using Bittrex.Net.Interfaces.Clients.Socket;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Options for the Bittrex client
    /// </summary>
    public class BittrexClientOptions : RestClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexClientOptions Default { get; set; } = new BittrexClientOptions()
        {
            BaseAddress = "https://api.bittrex.com"
        };

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
        }
    }
    
    /// <summary>
    /// Options for the Bittrex socket client
    /// </summary>
    public class BittrexSocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexSocketClientOptions Default { get; set; } = new BittrexSocketClientOptions()
        {
            BaseAddress = "https://socket-v3.bittrex.com",
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexSocketClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
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
        public IBittrexSocketClient? SocketClient { get; }

        /// <summary>
        /// The rest client to use for requesting the initial order book
        /// </summary>
        public IBittrexClient? RestClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="socketClient">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        /// <param name="restClient">The client to use for the initial order book request.</param>
        public BittrexOrderBookOptions(IBittrexSocketClient? socketClient = null, IBittrexClient? restClient = null)
        {
            SocketClient = socketClient;
            RestClient = restClient;
        }
    }
}
