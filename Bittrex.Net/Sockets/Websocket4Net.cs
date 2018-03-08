using Bittrex.Net.Interfaces;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using SuperSocket.ClientEngine.Proxy;
using WebSocket4Net;

namespace Bittrex.Net.Sockets
{
    public class Websocket4Net : IWebsocket
    {
        private readonly List<Action<Exception>> errorhandlers = new List<Action<Exception>>();
        private readonly List<Action> openhandlers = new List<Action>();
        private readonly List<Action> closehandlers = new List<Action>();
        private readonly List<Action<string>> messagehandlers = new List<Action<string>>();
        private readonly WebSocket socket;
        private readonly Uri url;

        public Websocket4Net(string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            this.url = new Uri(url);

            socket = new WebSocket(url, cookies: cookies.ToList(), customHeaderItems: headers.ToList(), receiveBufferSize: 2048, sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls) {NoDelay = true};

            socket.Error += HandleError;
            socket.Opened += HandleOpen;
            socket.Closed += HandleClose;
            socket.MessageReceived += HandleMessage;
        }

        private void HandleError(object sender, ErrorEventArgs e)
        {
            foreach (var handler in new List<Action<Exception>>(errorhandlers))
                handler(e.Exception);
        }

        private void HandleOpen(object sender, EventArgs e)
        {
            foreach (var handler in new List<Action>(openhandlers))
                handler();
        }

        private void HandleClose(object sender, EventArgs e)
        {
            foreach (var handler in new List<Action>(closehandlers))
                handler();
        }

        private void HandleMessage(object sender, MessageReceivedEventArgs e)
        {
            foreach (var handler in new List<Action<string>>(messagehandlers))
                handler(e.Message);
        }

        public event Action<Exception> OnError
        {
            add => errorhandlers.Add(value);
            remove => errorhandlers.Remove(value);
        }
        public event Action OnOpen
        {
            add => openhandlers.Add(value);
            remove => openhandlers.Remove(value);
        }
        public event Action OnClose
        {
            add => closehandlers.Add(value);
            remove => closehandlers.Remove(value);
        }
        public event Action<string> OnMessage
        {
            add => messagehandlers.Add(value);
            remove => messagehandlers.Remove(value);
        }

        public void Close()
        {
            socket.Close();
        }

        public void SetProxy(IWebProxy connectionProxy)
        {
            var proxy = connectionProxy.GetProxy(url);
            socket.Security.AllowNameMismatchCertificate = true;
            socket.Security.AllowUnstrustedCertificate = true;

            IPAddress address;
            socket.Proxy = IPAddress.TryParse(proxy.Host, out address) 
                ? new HttpConnectProxy(new IPEndPoint(address, proxy.Port)) 
                : new HttpConnectProxy(new DnsEndPoint(proxy.Host, proxy.Port));
        }

        public bool IsClosed()
        {
            return socket.State == WebSocketState.Closed;
        }

        public bool IsOpen()
        {
            return socket.State == WebSocketState.Open;
        }

        public async Task Open()
        {
            await socket.OpenAsync();
        }

        public void Send(string data)
        {
            socket.Send(data);
        }
    }
}