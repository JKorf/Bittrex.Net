using System.Net;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            HubConnection hubConnection = new HubConnection(url);
            return new BittrexHubConnection(hubConnection);
        }
    }
}
