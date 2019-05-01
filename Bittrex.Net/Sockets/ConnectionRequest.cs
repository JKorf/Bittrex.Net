using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Sockets
{
    internal class ConnectionRequest
    {
        public string RequestName { get; set; }
        public string[] Parameters { get; set; }


        public ConnectionRequest(string name, params string[] parameters)
        {
            RequestName = name;
            Parameters = parameters;
        }
    }
}
