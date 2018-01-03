using Bittrex.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.Sockets
{
    public class WebsocketNative : IWebsocket
    {
        List<Action<Exception>> errorhandlers = new List<Action<Exception>>();
        List<Action> openhandlers = new List<Action>();
        List<Action> closehandlers = new List<Action>();
        List<Action<string>> messagehandlers = new List<Action<string>>();
#if NETSTANDARD
        ClientWebSocket socket;
#else
        CustomWebsocket socket;
#endif
        string url;
        CancellationTokenSource tokenSource;

        public WebsocketNative(string url, string cookieHeader, string userAgent)
        {
            tokenSource = new CancellationTokenSource();

#if NETSTANDARD
            socket = new ClientWebSocket();
            socket.Options.SetRequestHeader("Cookie", cookieHeader);
            socket.Options.SetRequestHeader("User-Agent", userAgent);           
#else
            socket = new CustomWebsocket();
            socket.Options.RequestHeaders.Add("Cookie", cookieHeader);
            socket.Options.RequestHeaders.Add("User-Agent", userAgent);
#endif


            this.url = url;
        }

        private void HandleError(Exception e)
        {
            foreach (var handler in errorhandlers.ToList())
                handler(e);
        }

        private void HandleOpen()
        {
            foreach (var handler in openhandlers.ToList())
                handler();
        }

        private void HandleClose()
        {
            foreach (var handler in closehandlers.ToList())
                handler();
        }

        private void HandleMessage(string data)
        {
            foreach (var handler in messagehandlers.ToList())
                handler(data);
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
            try
            {
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", tokenSource.Token).Wait(500);
            }
            catch (Exception)
            {
            }
            HandleClose();
        }

        public bool IsClosed()
        {
            return socket.State == WebSocketState.Closed;
        }

        public bool IsOpen()
        {
            return socket.State == WebSocketState.Open;
        }

        public void Open()
        {
            try
            {
                socket.ConnectAsync(new Uri(url), tokenSource.Token).Wait();
                Task.Run(() => ProcessReceive());
                HandleOpen();
            }
            catch (Exception)
            {
                HandleClose();
            }
        }

        private void ProcessReceive()
        {
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    byte[] dataArray = new byte[100000];
                    var buffer = new ArraySegment<byte>(dataArray);
                    var result = socket.ReceiveAsync(buffer, tokenSource.Token).Result;
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        HandleClose();
                        return;
                    }

                    int read = result.Count;
                    while (!result.EndOfMessage)
                    {
                        if (read >= dataArray.Length)
                        {
                            socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", tokenSource.Token).Wait();
                            HandleClose();
                            return;
                        }

                        buffer = new ArraySegment<byte>(dataArray, read, dataArray.Length - read);
                        result = socket.ReceiveAsync(buffer, tokenSource.Token).Result;
                        read += result.Count;
                    }
                    
                    HandleMessage(Encoding.ASCII.GetString(dataArray, 0, read));
                }
            }
            catch (Exception e)
            {
                HandleError(e);
                Close();
            }
        }

        public void Send(string data)
        {
            try
            {
                socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(data)), WebSocketMessageType.Text, true, tokenSource.Token).Wait();
            }
            catch (Exception e)
            {
                HandleError(e);
                Close();
            }
        }
    }
}
