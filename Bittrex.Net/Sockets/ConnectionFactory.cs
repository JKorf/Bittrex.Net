using Bittrex.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(Log log, string url)
        {
            return new BittrexHubConnection(log, new HubConnection(url));
        }
    }
}
