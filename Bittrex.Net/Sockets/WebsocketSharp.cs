#if !NETSTANDARD
using Bittrex.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using WebSocketSharp;

namespace Bittrex.Net.Sockets
{
    public class WebsocketSharp : IWebsocket
    {
        List<Action<Exception>> errorhandlers = new List<Action<Exception>>();
        List<Action> openhandlers = new List<Action>();
        List<Action> closehandlers = new List<Action>();
        List<Action<string>> messagehandlers = new List<Action<string>>();
        WebSocket socket;

        public WebsocketSharp(string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            socket = new WebSocket(url);
            socket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            socket.CustomHeaders = headers;

            foreach(var kvp in cookies)
                socket.SetCookie(new WebSocketSharp.Net.Cookie(kvp.Key, kvp.Value));

            socket.OnError += HandleError;
            socket.OnOpen += HandleOpen;
            socket.OnClose += HandleClose;
            socket.OnMessage += HandleMessage;
        }

        private void HandleError(object sender, ErrorEventArgs e)
        {
            foreach (var handler in errorhandlers)
                handler(e.Exception);
        }

        private void HandleOpen(object sender, EventArgs e)
        {
            foreach (var handler in openhandlers)
                handler();
        }

        private void HandleClose(object sender, CloseEventArgs e)
        {
            foreach (var handler in closehandlers)
                handler();
        }

        private void HandleMessage(object sender, MessageEventArgs e)
        {
            foreach (var handler in messagehandlers)
                handler(e.Data);
        }

        public event Action<Exception> OnError
        {
            add { errorhandlers.Add(value); }
            remove { errorhandlers.Remove(value); }
        }
        public event Action OnOpen
        {
            add { openhandlers.Add(value); }
            remove { openhandlers.Remove(value); }
        }
        public event Action OnClose
        {
            add { closehandlers.Add(value); }
            remove { closehandlers.Remove(value); }
        }
        public event Action<string> OnMessage
        {
            add { messagehandlers.Add(value); }
            remove { messagehandlers.Remove(value); }
        }
        
        public void Close()
        {
            socket.Close();
        }

        public bool IsClosed()
        {
            return socket.ReadyState == WebSocketState.Closed;
        }

        public bool IsOpen()
        {
            return socket.ReadyState == WebSocketState.Open;
        }

        public void Open()
        {
            socket.Connect();
        }

        public void Send(string data)
        {
            socket.Send(data);
        }
    }
}
#endif