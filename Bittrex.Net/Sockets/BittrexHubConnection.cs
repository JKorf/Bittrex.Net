using Bittrex.Net.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace Bittrex.Net.Sockets
{
    public class BittrexHubConnection: IHubConnection
    {
        HubConnection connection;

        public BittrexHubConnection(HubConnection connection)
        {
            this.connection = connection;
        }

        public ConnectionState State => connection.State;
        
        public event Action<StateChange> StateChanged
        {
            add => connection.StateChanged += value;
            remove => connection.StateChanged -= value;
        }

        public event Action Closed
        {
            add => connection.Closed += value;
            remove => connection.Closed -= value;
        }

        public event Action<Exception> Error
        {
            add => connection.Error += value;
            remove => connection.Error -= value;
        }

        public event Action ConnectionSlow
        {
            add => connection.ConnectionSlow += value;
            remove => connection.ConnectionSlow -= value;
        }

        public CookieContainer Cookies
        {
            get => connection.CookieContainer;
            set => connection.CookieContainer = value;
        }

        public string UserAgent
        {
            get => connection.Headers["User-Agent"];
            set => connection.Headers["User-Agent"] = value;
        }

        public IHubProxy CreateHubProxy(string hubName)
        {
            return connection.CreateHubProxy(hubName);
        }

        public Task Start()
        {
            var client = new HttpClientWithUserAgent();
            var autoTransport = new AutoTransport(client, new IClientTransport[] {
                new LongPollingTransport(client),
                new WebsocketCustomTransport(client),
            });
            connection.TransportConnectTimeout = new TimeSpan(0, 0, 10);
            return connection.Start(autoTransport);
        }

        public void Stop(TimeSpan timeout)
        {
            connection.Stop(timeout);
        }
    }
}
