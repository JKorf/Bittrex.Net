using Bittrex.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

namespace Bittrex.Net.Objects.Internal
{
    internal class BittrexHubConnection : ISignalRSocket
    {
        private readonly HubConnection _connection;
        private readonly WebsocketCustomTransport _transport;
        private readonly ILogger _logger;
        private IHubProxy? _hubProxy;

        public event Action? OnClose;
        public event Action<string>? OnMessage;
        public event Action<int>? OnRequestSent;
        public event Action<Exception>? OnError;
        public event Action? OnOpen;
        public event Action? OnReconnecting;
        public event Action? OnReconnected;

        public bool IsOpen => _connection.State == ConnectionState.Connected;
        public int Id => _transport.Socket.Id;
        public double IncomingKbps => _transport.Socket.IncomingKbps;
        public Uri Uri => _transport.Socket.Uri;
        public bool IsClosed => _transport.Socket.IsClosed;
        public Func<Task<Uri?>>? GetReconnectionUrl { get; set; }

        public BittrexHubConnection(ILogger logger, WebSocketParameters parameters)
        {
            _connection = new HubConnection(parameters.Uri.ToString());
            _logger = logger;

            var client = new DefaultHttpClient();
            _transport = new WebsocketCustomTransport(_logger, client, parameters);
            _transport.Socket.OnReconnecting += () => OnReconnecting?.Invoke();
            _transport.Socket.OnReconnected += () => OnReconnected?.Invoke();
            _transport.Socket.OnOpen += () => OnOpen?.Invoke();
            _transport.Socket.OnClose += () => OnClose?.Invoke();
            _transport.Socket.OnError += (a) => OnError?.Invoke(a);
            _transport.Socket.OnRequestSent += (a) => OnRequestSent?.Invoke(a);
            // Messages will be received via the connection to make sure SignalR knows about them and can handle heartbeat and timeouts
            _connection.Received += (str) => OnMessage?.Invoke(str);

            if (parameters.Proxy != null)
            {
                _connection.Proxy = new WebProxy(parameters.Proxy.Host, parameters.Proxy.Port);
                if (!string.IsNullOrEmpty(parameters.Proxy.Login))
                    _connection.Proxy.Credentials = new NetworkCredential(parameters.Proxy.Login, parameters.Proxy.Password);
            }

        }

        public void SetHub(string name)
        {
            _hubProxy = _connection.CreateHubProxy(name);
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
                    _logger.Log(LogLevel.Debug, $"Socket {_transport.Socket.Id} sending data: {call}, {ArrayToString(pars)}");
                    var sub = await _hubProxy.Invoke<T>(call, pars).ConfigureAwait(false);
                    return new CallResult<T>(sub);
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Warning, $"Socket {_transport.Socket.Id} failed to invoke proxy, try {i}: " + (e.InnerException?.Message ?? e.Message));
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

        public async Task<bool> ConnectAsync()
        {
            _connection.TransportConnectTimeout = new TimeSpan(0, 0, 10);
            try
            {
                await _connection.Start(_transport).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                _connection.Stop(TimeSpan.FromSeconds(1));
            }).ConfigureAwait(false);
        }

        public void Send(int requestId, string data, int weight)
        {
        }

        public void Dispose()
        {
        }

        public Task ReconnectAsync() => _transport.Socket.ReconnectAsync();
    }
}
