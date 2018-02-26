using System.Net;

namespace Bittrex.Net.Interfaces
{
    public interface IRequest
    {
        WebHeaderCollection Headers { get; set; }
        string Method { get; set; }

        void SetProxy(string host, int port);
        IResponse GetResponse();
    }
}
