using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            return new BittrexHubConnection(new HubConnection(url));
        }
    }
}
