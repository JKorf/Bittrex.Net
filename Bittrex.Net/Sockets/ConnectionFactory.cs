using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class ConnectionFactory : IConnectionFactory
    {
        public IHubConnection Create(string url)
        {
            HubConnection hubConnection = new HubConnection(
                "https://socket.bittrex.com/signalr/", // https://socket.bittrex.com/signalr/connect
                useDefaultUrl: false,
                queryString: "tid=1"
                // ,queryString: "transport=webSockets&clientProtocol=1.5"
                );
            return new BittrexHubConnection(hubConnection);
        }
    }
}
