using System.Net;
using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            HubConnection hubConnection = createHubConnection(url);
            return new BittrexHubConnection(hubConnection);
        }

        public IHubConnection Create(string url, string proxyDns, int proxyPort)
        {
            HubConnection hubConnection = createHubConnection(url);
            hubConnection.Proxy = new WebProxy(proxyDns, proxyPort);
            return new BittrexHubConnection(hubConnection);
        }

        private static HubConnection createHubConnection(string url)
        {
            HubConnection hubConnection = new HubConnection(
                url + "signalr",
                useDefaultUrl: false,
                queryString: "tid=1"
            );
            return hubConnection;
        }
    }
}
