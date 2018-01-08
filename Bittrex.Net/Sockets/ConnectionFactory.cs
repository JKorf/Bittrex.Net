using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            HubConnection hubConnection = new HubConnection(
                url + "signalr",
                useDefaultUrl: false,
                queryString: "tid=1"
                );
            return new BittrexHubConnection(hubConnection);
        }
    }
}
