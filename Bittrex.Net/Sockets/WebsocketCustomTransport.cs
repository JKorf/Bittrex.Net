using Bittrex.Net.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.Sockets
{
    public class WebsocketCustomTransport : ClientTransportBase
    {
        private IConnection _connection;
        private string _connectionData;
        private CancellationToken _disconnectToken;
        private CancellationTokenSource _webSocketTokenSource;
        private IWebsocket _websocket;
        private int _disposed;
        
        public TimeSpan ReconnectDelay { get; set; }

        public WebsocketCustomTransport()
      : this(new DefaultHttpClient())
        {
        }

        public WebsocketCustomTransport(IHttpClient client)
        : base(client, "webSockets")
        {
            _disconnectToken = CancellationToken.None;
            ReconnectDelay = TimeSpan.FromSeconds(2.0);
        }

        ~WebsocketCustomTransport()
        {
            Dispose(false);
        }

        protected override void OnStart(IConnection connection, string connectionData, CancellationToken disconnectToken)
        {
            _connection = connection;
            _connectionData = connectionData;
            _disconnectToken = disconnectToken;

            var connectUrl = UrlBuilder.BuildConnect(connection, Name, connectionData);

            try
            {
                PerformConnect(connectUrl);
            }
            catch (Exception ex)
            {
                TransportFailed(ex);
            }
        }

        protected override void OnStartFailed()
        {
            Dispose();
        }

        public override Task Send(IConnection connection, string data, string connectionData)
        {
            if (_websocket.IsOpen())
            {
                _websocket.Send(data);
                return Task.FromResult(0);
            }

            var ex = new InvalidOperationException("Socket closed");
            connection.OnError(ex);

            throw ex;
        }

        public override void LostConnection(IConnection connection)
        {
            _connection.Trace(TraceLevels.Events, "WS: LostConnection");

            if (_webSocketTokenSource == null)
            {
                return;
            }

            _webSocketTokenSource.Cancel();
        }

        public override bool SupportsKeepAlive
        {
            get { return true; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 1)
                {
                    base.Dispose(true);
                    return;
                }

                if (_webSocketTokenSource != null)
                {
                    _webSocketTokenSource.Cancel();
                }

                if (_websocket != null)
                {
                    DisposeWebSocket();
                }

                if (_webSocketTokenSource != null)
                {
                    _webSocketTokenSource.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void DisposeWebSocket()
        {
            _websocket.OnError -= WebSocketOnError;
            _websocket.OnOpen -= WebSocketOnOpened;
            _websocket.OnClose -= WebSocketOnClosed;
            _websocket.OnMessage -= WebSocketOnMessageReceived;

            _websocket = null;
        }

        private void PerformConnect(string url)
        {
            if (_websocket != null)
            {
                DisposeWebSocket();
            }

            _webSocketTokenSource = new CancellationTokenSource();
            _webSocketTokenSource.Token.Register(WebSocketTokenSourceCanceled);
            CancellationTokenSource.CreateLinkedTokenSource(_webSocketTokenSource.Token, _disconnectToken);

            // SignalR uses https, websocket4net uses wss
            url = url.Replace("http://", "ws://").Replace("https://", "wss://");

            IDictionary<string, string> cookies = new Dictionary<string, string>();
            if (_connection.CookieContainer != null)
            {
                var container = _connection.CookieContainer.GetCookies(new Uri(_connection.Url));
                foreach (Cookie cookie in container)
                    cookies.Add(cookie.Name, cookie.Value);
            }

            // Add the header from the connection to the socket connection
            var headers = _connection.Headers.ToList();

#if !NETSTANDARD
            _websocket = new WebsocketSharp(url, cookies, _connection.Headers);
#else
            _websocket = new Websocket4Net(url, cookies, _connection.Headers);
#endif           

            _websocket.OnError += WebSocketOnError;
            _websocket.OnOpen += WebSocketOnOpened;
            _websocket.OnClose += WebSocketOnClosed;
            _websocket.OnMessage += WebSocketOnMessageReceived;

            _websocket.Open();
        }

        private async Task DoReconnect()
        {
            string reconnectUrl = UrlBuilder.BuildReconnect(_connection, Name, _connectionData);

            while (TransportHelper.VerifyLastActive(_connection))
            {
                if (_connection.EnsureReconnecting())
                {
                    try
                    {
                        PerformConnect(reconnectUrl);
                        break;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _connection.OnError(ex);
                    }
                    await Task.Delay(ReconnectDelay, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }

        private void WebSocketOnOpened()
        {
            _connection.Trace(TraceLevels.Events, "WS: OnOpen()");

            if (!_connection.ChangeState(ConnectionState.Reconnecting, ConnectionState.Connected))
            {
                return;
            }
            _connection.OnReconnected();
        }

        private async void WebSocketOnClosed()
        {
            _connection.Trace(TraceLevels.Events, "WS: OnClose()");

            if (_disconnectToken.IsCancellationRequested || AbortHandler.TryCompleteAbort())
            {
                return;
            }

            await DoReconnect();
        }

        private void WebSocketOnError(Exception e)
        {
            _connection.OnError(e);
        }

        private void WebSocketOnMessageReceived(string data)
        {
            _connection.Trace(TraceLevels.Messages, "WS: OnMessage({0})", (object)data);
            ProcessResponse(_connection, data);
        }

        private void WebSocketTokenSourceCanceled()
        {
            if (_webSocketTokenSource.IsCancellationRequested)
            {
                if (_websocket != null && !_websocket.IsClosed())
                {
                    _websocket.Close();
                }
            }
        }
    }
}
