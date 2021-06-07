using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    internal class ConnectionFactory : IWebsocketFactory
    {
        private ApiProxy? proxy;

        public ConnectionFactory(ApiProxy? proxy)
        {
            proxy = proxy;
        }

        public IWebsocket CreateWebsocket(Log log, string url)
        {
            return new BittrexHubConnection(log, proxy, new HubConnection(url));
        }

        public IWebsocket CreateWebsocket(Log log, string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            throw new System.NotImplementedException();
        }
    }
}
