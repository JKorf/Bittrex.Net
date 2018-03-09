using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Implementation;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Sockets
{
    public class WebsocketCustomTransport : ClientTransportBase
    {
        private IConnection connection;
        private string connectionData;
        private CancellationToken disconnectToken;
        private CancellationTokenSource webSocketTokenSource;
        private IWebsocket websocket;
        private int disposed;
        
        public TimeSpan ReconnectDelay { get; set; }

        public WebsocketCustomTransport(): this(new DefaultHttpClient())
        {
        }

        public WebsocketCustomTransport(IHttpClient client): base(client, "webSockets")
        {
            disconnectToken = CancellationToken.None;
            ReconnectDelay = TimeSpan.FromSeconds(2.0);
        }

        ~WebsocketCustomTransport()
        {
            Dispose(false);
        }

        protected override void OnStart(IConnection con, string conData, CancellationToken disconToken)
        {
            connection = con;
            connectionData = conData;
            disconnectToken = disconToken;

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

        public override Task Send(IConnection con, string data, string conData)
        {
            if (websocket.IsOpen)
            {
                websocket.Send(data);
                return Task.FromResult(0);
            }

            var ex = new InvalidOperationException("Socket closed");
            connection.OnError(ex);

            throw ex;
        }

        public override void LostConnection(IConnection con)
        {
            connection.Trace(TraceLevels.Events, "WS: LostConnection");

            webSocketTokenSource?.Cancel();
        }

        public override bool SupportsKeepAlive => true;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Interlocked.Exchange(ref disposed, 1) == 1)
                {
                    base.Dispose(true);
                    return;
                }

                webSocketTokenSource?.Cancel();

                if (websocket != null)
                {
                    DisposeWebSocket();
                }

                webSocketTokenSource?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void DisposeWebSocket()
        {
            websocket.OnError -= WebSocketOnError;
            websocket.OnOpen -= WebSocketOnOpened;
            websocket.OnClose -= WebSocketOnClosed;
            websocket.OnMessage -= WebSocketOnMessageReceived;

            websocket = null;
        }

        private void PerformConnect(string url)
        {
            if (websocket != null)
                DisposeWebSocket();

            webSocketTokenSource = new CancellationTokenSource();
            webSocketTokenSource.Token.Register(WebSocketTokenSourceCanceled);
            CancellationTokenSource.CreateLinkedTokenSource(webSocketTokenSource.Token, disconnectToken);

            // SignalR uses https, websocket4net uses wss
            url = url.Replace("http://", "ws://").Replace("https://", "wss://");

            IDictionary<string, string> cookies = new Dictionary<string, string>();
            if (connection.CookieContainer != null)
            {
                var container = connection.CookieContainer.GetCookies(new Uri(connection.Url));
                foreach (Cookie cookie in container)
                    cookies.Add(cookie.Name, cookie.Value);
            }
    
            websocket = new BaseSocket(url, cookies, connection.Headers);
            websocket.OnError += WebSocketOnError;
            websocket.OnOpen += WebSocketOnOpened;
            websocket.OnClose += WebSocketOnClosed;
            websocket.OnMessage += WebSocketOnMessageReceived;

            if (connection.Proxy != null)
            {
                var proxy = connection.Proxy.GetProxy(new Uri(url));
                websocket.SetProxy(proxy.Host, proxy.Port);
            }

            websocket.Connect().Wait(webSocketTokenSource.Token);
        }

        private async Task DoReconnect()
        {
            var reconnectUrl = UrlBuilder.BuildReconnect(connection, Name, connectionData);

            while (TransportHelper.VerifyLastActive(connection))
            {
                if (connection.EnsureReconnecting())
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
                        connection.OnError(ex);
                    }
                    await Task.Delay(ReconnectDelay, CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    break;
                }
            }
        }

        private void WebSocketOnOpened()
        {
            connection.Trace(TraceLevels.Events, "WS: OnOpen()");

            if (!connection.ChangeState(ConnectionState.Reconnecting, ConnectionState.Connected))
            {
                return;
            }
            connection.OnReconnected();
        }

        private async void WebSocketOnClosed()
        {
            connection.Trace(TraceLevels.Events, "WS: OnClose()");

            if (disconnectToken.IsCancellationRequested || AbortHandler.TryCompleteAbort())
            {
                return;
            }

            await DoReconnect().ConfigureAwait(false);
        }

        private void WebSocketOnError(Exception e)
        {
            connection.OnError(e);
        }

        private void WebSocketOnMessageReceived(string data)
        {
            connection.Trace(TraceLevels.Messages, "WS: OnMessage({0})", (object)data);
            ProcessResponse(connection, data);
        }

        private void WebSocketTokenSourceCanceled()
        {
            if (!webSocketTokenSource.IsCancellationRequested)
                return;

            if (websocket != null && !websocket.IsClosed)
                websocket.Close();
        }
    }
}
