using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Objects.Internal
{
    internal class BittrexWebsocketFactory : IWebsocketFactory
    {
        public IWebsocket CreateWebsocket(Log log, WebSocketParameters parameters)
        {
            return new BittrexHubConnection(log, parameters);
        }
    }
}
