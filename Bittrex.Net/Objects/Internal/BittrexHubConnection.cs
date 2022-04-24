using Bittrex.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Client.Http;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

namespace Bittrex.Net.Objects.Internal
{
    internal class BittrexHubConnection : CryptoExchangeWebSocketClient, ISignalRSocket
    {
        private readonly HubConnection _connection;
        private IHubProxy? _hubProxy;
        private readonly ApiProxy? _proxy;

        public new bool IsOpen => _connection.State == ConnectionState.Connected;

        public BittrexHubConnection(Log log, ApiProxy? proxy, HubConnection connection) : base(null!, new Uri(connection.Url))
        {
            this._connection = connection;
            this.log = log;
            this._proxy = proxy;

            connection.StateChanged += StateChangeHandler;
            connection.Error += s => Handle(errorHandlers, s);
            connection.Received += str =>
            {
                lock (_receivedMessagesLock)
                {
                    UpdateReceivedMessages();
                    _receivedMessages.Add(new ReceiveItem(DateTime.UtcNow, str.Length));
                }
                Handle(messageHandlers, str);
            };
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
                    _connection.Stop(TimeSpan.FromMilliseconds(100));
                    break;
            }
        }

        public void SetHub(string name)
        {
            _hubProxy = _connection.CreateHubProxy(name);
        }

        public override void SetProxy(ApiProxy proxy)
        {
            _connection.Proxy = new WebProxy(proxy.Host, proxy.Port);
            if (!string.IsNullOrEmpty(proxy.Login))
                _connection.Proxy.Credentials = new NetworkCredential(proxy.Login, proxy.Password);
        }

        public async Task<CallResult<T>> InvokeProxy<T>(string call, params object[] pars)
        {
            if (_hubProxy == null)
                throw new InvalidOperationException("HubProxy not set");

            Error? error = null;
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    log.Write(LogLevel.Debug, $"Socket {Id} sending data: {call}, {ArrayToString(pars)}");
                    var sub = await _hubProxy.Invoke<T>(call, pars).ConfigureAwait(false);
                    return new CallResult<T>(sub);
                }
                catch (Exception e)
                {
                    log.Write(LogLevel.Warning, $"Socket {Id} failed to invoke proxy, try {i}: " + (e.InnerException?.Message ?? e.Message));
                    error = new UnknownError("Failed to invoke proxy: " + (e.InnerException?.Message ?? e.Message));
                }
            }

            return new CallResult<T>(error!);
        }

        private string ArrayToString(object item)
        {
            if (!item.GetType().IsArray)
                return item.ToString();

            return $"[{ string.Join(", ", ItemToString((Array)item))}]";
        }

        private IEnumerable<string> ItemToString(Array item)
        {
            var result = new List<string>();
            foreach (var subItem in item)
                result.Add(ArrayToString(subItem));
            return result;
        }

        public override async Task<bool> ConnectAsync()
        {
            var client = new DefaultHttpClient();
            var autoTransport = new AutoTransport(client, new IClientTransport[] {
                new WebsocketCustomTransport(log, client, _proxy, DataInterpreterString)
            });
            _connection.TransportConnectTimeout = new TimeSpan(0, 0, 10);
            try
            {
                await _connection.Start(autoTransport).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                _connection.Stop(TimeSpan.FromSeconds(1));
            }).ConfigureAwait(false);
        }
    }
}
