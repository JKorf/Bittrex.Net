using System.Net.Http;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Interfaces.Clients.Rest.Spot;
using Bittrex.Net.Interfaces.Clients.Socket;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Options for the Bittrex client
    /// </summary>
    public class BittrexClientSpotOptions : RestClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexClientSpotOptions Default { get; set; } = new BittrexClientSpotOptions()
        {
            BaseAddress = "https://api.bittrex.com"
        };

        /// <summary>
        /// The V2 API base address
        /// </summary>
        //public string BaseAddressV2 { get; set; } = "https://international.bittrex.com";

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexClientSpotOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
        }
    }
    
    /// <summary>
    /// Options for the Bittrex socket client
    /// </summary>
    public class BittrexSocketClientSpotOptions : SocketClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexSocketClientSpotOptions Default { get; set; } = new BittrexSocketClientSpotOptions()
        {
            BaseAddress = "https://socket-v3.bittrex.com",
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexSocketClientSpotOptions()
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
        public IBittrexSocketClientSpot? SocketClient { get; }

        /// <summary>
        /// The rest client to use for requesting the initial order book
        /// </summary>
        public IBittrexClientSpot? RestClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="socketClient">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        /// <param name="restClient">The client to use for the initial order book request.</param>
        public BittrexOrderBookOptions(IBittrexSocketClientSpot? socketClient = null, IBittrexClientSpot? restClient = null)
        {
            SocketClient = socketClient;
            RestClient = restClient;
        }
    }
}
