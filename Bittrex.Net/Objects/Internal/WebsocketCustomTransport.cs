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
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net;

namespace Bittrex.Net.Objects.Internal
{
    internal class WebsocketCustomTransport : ClientTransportBase
    {
        private readonly ILogger _logger;
        private readonly IWebsocket _websocket;
        private readonly WebSocketParameters _parameters;

        private IConnection? _connection;
        private string? _connectionData;
        private bool _started = false;

        public override bool SupportsKeepAlive => true;
        public IWebsocket Socket => _websocket;

        public WebsocketCustomTransport(ILogger log, IHttpClient client, WebSocketParameters parameters) : base(client, "webSockets")
        {
            _logger = log;
            _parameters = parameters;

            _websocket = new CryptoExchangeWebSocketClient(_logger, _parameters);
            _websocket.OnMessage += WebSocketOnMessageReceived;
        }

        ~WebsocketCustomTransport()
        {
            Dispose(false);
        }

        protected override void OnStart(IConnection con, string conData, CancellationToken disconToken)
        {
            if (_started)
                return;

            _started = true;
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

            _parameters.Uri = new Uri(connectUrl);
            _parameters.Cookies = cookies;
            _parameters.Headers = _connection.Headers;

            _ = Task.Run(async () =>
            {
                var connectResult = await _websocket.ConnectAsync().ConfigureAwait(false);
                if (!connectResult)
                    TryFailStart(new Exception("Failed to connect"));
            });
        }

        public override Task Send(IConnection con, string data, string conData)
        {
            _websocket.Send(ExchangeHelpers.NextId(), data, 1);
            return Task.CompletedTask;
        }

        private void WebSocketOnMessageReceived(string data) => ProcessResponse(_connection, data);
        
        public override void Abort(IConnection connection, TimeSpan timeout, string connectionData) => _websocket.CloseAsync().Wait();

        protected override void OnStartFailed() { }
        public override void LostConnection(IConnection con) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _websocket.CloseAsync().Wait();
                _websocket.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
