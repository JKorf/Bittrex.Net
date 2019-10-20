
namespace Bittrex.Net.Sockets
{
    internal class ConnectionRequest
    {
        public string RequestName { get; set; }
        public object[] Parameters { get; set; }


        public ConnectionRequest(string name, params object[] parameters)
        {
            RequestName = name;
            Parameters = parameters;
        }
    }
}
