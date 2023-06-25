using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace Bittrex.Net.Objects.Internal
{
    internal class BittrexWebsocketFactory : IWebsocketFactory
    {
        public IWebsocket CreateWebsocket(ILogger logger, WebSocketParameters parameters)
        {
            return new BittrexHubConnection(logger, parameters);
        }
    }
}
