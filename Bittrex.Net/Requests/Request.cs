using System.Net;
using Bittrex.Net.Interfaces;

namespace Bittrex.Net.Requests
{
    public class Request : IRequest
    {
        private readonly WebRequest request;

        public Request(WebRequest request)
        {
            this.request = request;
        }

        public WebHeaderCollection Headers
        {
            get => request.Headers;
            set => request.Headers = value;
        }
        public string Method
        {
            get => request.Method;
            set => request.Method = value;
        }

        public IResponse GetResponse()
        {
            return new Response(request.GetResponse());
        }
    }
}
