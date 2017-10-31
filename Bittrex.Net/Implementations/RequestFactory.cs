using System.Net;
using Bittrex.Net.Interfaces;

namespace Bittrex.Net.Implementations
{
    public class RequestFactory : IRequestFactory
    {
        public IRequest Create(string uri)
        {
            return new Request(WebRequest.Create(uri));
        }
    }
}
