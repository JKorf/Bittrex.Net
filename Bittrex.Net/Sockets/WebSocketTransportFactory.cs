using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Net.WebSockets;

namespace Bittrex.Net.Sockets
{
    public static class WebSocketTransportFactory
    {
        public static IClientTransport Create()
        {
            IClientTransport clientTransport;

            try
            {
                // Test if .net websockets are supported
                // Supported since Windows 8 and newer
                var testSocket = new ClientWebSocket();
                clientTransport = new AutoTransport(new DefaultHttpClient());
            }
            catch (PlatformNotSupportedException)
            {
                clientTransport = new WebsocketCustomTransport();
            }

            return clientTransport;
        }
    }
}
