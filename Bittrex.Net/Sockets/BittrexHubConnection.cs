using Bittrex.Net.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Client.Http;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Sockets
{
    public class BittrexHubConnection: BaseSocket, ISignalRSocket
    {
        private readonly HubConnection connection;
        private IHubProxy hubProxy;
        public new string Url { get; }
        
        public BittrexHubConnection(Log log, HubConnection connection): base(null, connection.Url)
        {
            Url = connection.Url;
            this.connection = connection;
            this.log = log;

            connection.StateChanged += StateChangeHandler;
            connection.Error += s => Handle(errorHandlers, s);
            connection.Received += str => Handle(messageHandlers, str);
        }

        private void StateChangeHandler(StateChange change)
        {
            switch (change.NewState)
            {
                case ConnectionState.Connected:
                    Handle(openHandlers);
                    break;
                case ConnectionState.Disconnected:
                    Handle(closeHandlers);
                    break;
                case ConnectionState.Reconnecting:
                    connection.Stop(TimeSpan.FromMilliseconds(100));
                    break;
            }
        }        

        public void SetHub(string name)
        {
            hubProxy = connection.CreateHubProxy(name);
        }

        public override void SetProxy(string proxyHost, int proxyPort)
        {
            connection.Proxy = new WebProxy(proxyHost, proxyPort);
        }
        
        public async Task<CallResult<T>> InvokeProxy<T>(string call, params string[] pars)
        {
            try
            {
                log.Write(LogVerbosity.Debug, $"Sending data: {call}, [{string.Join(", ", pars)}]");
                var sub = await hubProxy.Invoke<T>(call, pars).ConfigureAwait(false);
                return new CallResult<T>(sub, null);
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Warning, "Failed to invoke proxy: " + e.Message);
                return new CallResult<T>(default(T), new UnknownError("Failed to invoke proxy: " + e.Message));
            }
        }

        public override async Task<bool> Connect()
        {
            var client = new DefaultHttpClient();
            var autoTransport = new AutoTransport(client, new IClientTransport[] {
                new WebsocketCustomTransport(log, client, DataInterpreterString)
            });
            connection.TransportConnectTimeout = new TimeSpan(0, 0, 10);
            try
            {
                await connection.Start(autoTransport).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task Close()
        {
            await Task.Run(() =>
            {
                connection.Stop(TimeSpan.FromSeconds(1));
            }).ConfigureAwait(false);
        }
    }
}
