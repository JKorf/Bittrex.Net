using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Sockets
{
    internal class ConnectionRequest: SocketRequest
    {
        public string RequestName { get; set; }
        public string[] Parameters { get; set; }

        public ConnectionRequest(bool authenticated, string name, params string[] parameters)
        {
            RequestName = name;
            Signed = authenticated;
            Parameters = parameters;
        }
    }
}
