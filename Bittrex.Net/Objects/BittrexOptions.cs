using CryptoExchange.Net;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    public class BittrexClientOptions : ClientOptions
    {
        public BittrexClientOptions()
        {
            BaseAddress = "https://api.bittrex.com";
        }

        public string BaseAddressV2 { get; set; } = "https://international.bittrex.com";
    }

    public class BittrexSocketClientOptions : SocketClientOptions
    {
        public BittrexSocketClientOptions()
        {
            BaseAddress = "https://socket.bittrex.com/signalr";
        }
    }
}
