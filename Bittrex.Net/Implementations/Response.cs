using System.IO;
using System.Net;
using Bittrex.Net.Interfaces;

namespace Bittrex.Net.Implementations
{
    public class Response : IResponse
    {
        private readonly WebResponse response;

        public Response(WebResponse response)
        {
            this.response = response;
        }

        public Stream GetResponseStream()
        {
            return response.GetResponseStream();
        }
    }
}
