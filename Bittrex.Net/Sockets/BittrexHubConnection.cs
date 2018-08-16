using Bittrex.Net.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Client.Http;
using CryptoExchange.Net.Logging;

namespace Bittrex.Net.Sockets
{
    public class BittrexHubConnection: IHubConnection
    {
        private readonly HubConnection connection;
        private readonly Log log;

        public BittrexHubConnection(Log log, HubConnection connection)
        {
            this.connection = connection;
            this.log = log;
        }

        public void SetProxy(string proxyHost, int proxyPort)
        {
            connection.Proxy = new WebProxy(proxyHost, proxyPort);
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
            var client = new DefaultHttpClient();
            var autoTransport = new AutoTransport(client, new IClientTransport[] {
                new WebsocketCustomTransport(log, client)
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
