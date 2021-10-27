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
using Microsoft.Extensions.Logging;

namespace Bittrex.Net.Sockets
{
    internal class WebsocketCustomTransport : ClientTransportBase
    {
        private IConnection? connection;
        private string? connectionData;
        private IWebsocket? websocket;
        private ApiProxy? proxy;
        private readonly Log log;
        private readonly Func<string, string>? interpreter;

        public override bool SupportsKeepAlive => true;

        public WebsocketCustomTransport(Log log, IHttpClient client, ApiProxy? proxy, Func<string, string>? interpreter): base(client, "webSockets")
        {
            this.log = log;
            this.proxy = proxy;
            this.interpreter = interpreter;
        }

        ~WebsocketCustomTransport()
        {
            Dispose(false);
        }

        protected override void OnStart(IConnection con, string conData, CancellationToken disconToken)
        {
            connection = con;
            connectionData = conData;

            var connectUrl = UrlBuilder.BuildConnect(connection, Name, connectionData);

            if (websocket != null)
                DisposeWebSocket();

            // SignalR uses https, websocket4net uses wss
            connectUrl = connectUrl.Replace("http://", "ws://").Replace("https://", "wss://");

            IDictionary<string, string> cookies = new Dictionary<string, string>();
            if (connection.CookieContainer != null)
            {
                var container = connection.CookieContainer.GetCookies(new Uri(connection.Url));
                foreach (Cookie cookie in container)
                    cookies.Add(cookie.Name, cookie.Value);
            }

            websocket = new CryptoExchangeWebSocketClient(log, connectUrl, cookies, connection.Headers);
            websocket.OnError += WebSocketOnError;
            websocket.OnClose += WebSocketOnClosed;
            websocket.OnMessage += WebSocketOnMessageReceived;
            websocket.DataInterpreterString = interpreter;

            if (proxy != null)            
                websocket.SetProxy(proxy);

            if (!websocket.ConnectAsync().Result)
                OnStartFailed();
        }

        protected override void OnStartFailed()
        {
            Dispose();
        }

        public override Task Send(IConnection con, string data, string conData)
        {
            if (websocket != null && websocket.IsOpen)
            {
                websocket.Send(data);
                return Task.FromResult(0);
            }

            var ex = new InvalidOperationException("Socket closed");
            connection?.OnError(ex);

            throw ex;
        }

        public override void LostConnection(IConnection con)
        {
            connection?.Trace(TraceLevels.Events, "WS: LostConnection");
            connection?.Stop();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (websocket != null)
                    DisposeWebSocket();
            }

            base.Dispose(disposing);
        }

        private void DisposeWebSocket()
        {
            if (websocket == null)
                return;

            websocket.OnError -= WebSocketOnError;
            websocket.OnClose -= WebSocketOnClosed;
            websocket.OnMessage -= WebSocketOnMessageReceived;

            websocket.Dispose();
            websocket = null;
        }

        private void WebSocketOnClosed()
        {
            connection?.Trace(TraceLevels.Events, "WS: OnClose()");
            connection?.Stop();
        }

        public override void Abort(IConnection connection, TimeSpan timeout, string connectionData)
        {
            if (websocket == null)
                return;

            var socket = websocket;
            websocket = null;

            socket.CloseAsync().Wait();
            socket.Dispose();
        }

        private void WebSocketOnError(Exception e)
        {
            connection?.OnError(e);
        }

        private void WebSocketOnMessageReceived(string data)
        {
            connection?.Trace(TraceLevels.Messages, "WS: OnMessage({0})", (object)data);
            ProcessResponse(connection, data);
        }
    }
}
