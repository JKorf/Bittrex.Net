using System.Net;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            HubConnection hubConnection = CreateHubConnection(url);
            return new BittrexHubConnection(hubConnection);
        }

        public IHubConnection Create(string url, string proxyDns, int proxyPort)
        {
            HubConnection hubConnection = CreateHubConnection(url);
            hubConnection.Proxy = new WebProxy(proxyDns, proxyPort);
            return new BittrexHubConnection(hubConnection);
        }

        private static HubConnection CreateHubConnection(string url)
        {
            HubConnection hubConnection = new HubConnection(
                url + "signalr"
            );
            return hubConnection;
        }
    }
}
