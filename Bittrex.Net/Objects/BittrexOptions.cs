using Bittrex.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    public class BittrexClientOptions : RestClientOptions
    {
        public BittrexClientOptions()
        {
            BaseAddress = "https://api.bittrex.com";
        }

        public string BaseAddressV2 { get; set; } = "https://international.bittrex.com";

        public BittrexClientOptions Copy()
        {
            var copy = Copy<BittrexClientOptions>();
            copy.BaseAddressV2 = BaseAddressV2;
            return copy;
        }
    }

    public class BittrexSocketClientOptions : SocketClientOptions
    {
        public BittrexSocketClientOptions()
        {
            BaseAddress = "https://socket.bittrex.com";
            SocketSubscriptionsCombineTarget = 10;
        }
    }

    public class BittrexOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IBittrexSocketClient SocketClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="socketClient">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        public BittrexOrderBookOptions(IBittrexSocketClient socketClient = null) : base("Bittrex", true)
        {
            SocketClient = socketClient;
        }
    }
}
