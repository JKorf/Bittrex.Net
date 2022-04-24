using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects.Internal
{
    internal class WebsocketCustomTransport : ClientTransportBase
    {
        private IConnection? _connection;
        private string? _connectionData;
        private IWebsocket? _websocket;
        private readonly ApiProxy? _proxy;
        private readonly Log _log;
        private readonly Func<string, string>? _interpreter;

        public override bool SupportsKeepAlive => true;

        public WebsocketCustomTransport(Log log, IHttpClient client, ApiProxy? proxy, Func<string, string>? interpreter) : base(client, "webSockets")
        {
            _log = log;
            _proxy = proxy;
            _interpreter = interpreter;
        }

        ~WebsocketCustomTransport()
        {
            Dispose(false);
        }

        protected override void OnStart(IConnection con, string conData, CancellationToken disconToken)
        {
            _connection = con;
            _connectionData = conData;

            var connectUrl = UrlBuilder.BuildConnect(_connection, Name, _connectionData);

            // SignalR uses https, but we need wss
            connectUrl = connectUrl.Replace("http://", "ws://").Replace("https://", "wss://");

            IDictionary<string, string> cookies = new Dictionary<string, string>();
            if (_connection.CookieContainer != null)
            {
                var container = _connection.CookieContainer.GetCookies(new Uri(_connection.Url));
                foreach (Cookie cookie in container)
                    cookies.Add(cookie.Name, cookie.Value);
            }

            if (_websocket != null)
            {
                _websocket.Reset();
            }
            else
            {
                _websocket = new CryptoExchangeWebSocketClient(_log, new Uri(connectUrl), cookies, _connection.Headers);
                _websocket.OnError += WebSocketOnError;
                _websocket.OnClose += WebSocketOnClosed;
                _websocket.OnMessage += WebSocketOnMessageReceived;
                _websocket.DataInterpreterString = _interpreter;

                if (_proxy != null)
                    _websocket.SetProxy(_proxy);
            }

            _ = Task.Run(async () =>
            {
                var connectResult = await _websocket.ConnectAsync().ConfigureAwait(false);
                if (!connectResult)
                    TryFailStart(new Exception("Failed to connect"));
            });
        }

        public override Task Send(IConnection con, string data, string conData)
        {
            if (_websocket != null && _websocket.IsOpen)
            {
                _websocket.Send(data);
                return Task.FromResult(0);
            }

            var ex = new InvalidOperationException("Socket closed");
            _connection?.OnError(ex);

            throw ex;
        }

        public override void LostConnection(IConnection con)
        {
            _connection?.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_websocket != null)
                    DisposeWebSocket();
            }

            base.Dispose(disposing);
        }

        private void DisposeWebSocket()
        {
            if (_websocket == null)
                return;

            _websocket.OnError -= WebSocketOnError;
            _websocket.OnClose -= WebSocketOnClosed;
            _websocket.OnMessage -= WebSocketOnMessageReceived;

            _websocket.Dispose();
            _websocket = null;
        }

        private void WebSocketOnClosed()
        {
            _connection?.Stop();
        }

        public override void Abort(IConnection connection, TimeSpan timeout, string connectionData)
        {
            if (_websocket == null)
                return;

            var socket = _websocket;
            _websocket = null;

            socket.CloseAsync().Wait();
            socket.Dispose();
        }

        private void WebSocketOnError(Exception e)
        {
            _connection?.OnError(e);
        }

        private void WebSocketOnMessageReceived(string data)
        {
            ProcessResponse(_connection, data);
        }

        protected override void OnStartFailed()
        {
        }
    }
}
