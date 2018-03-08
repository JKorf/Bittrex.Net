using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;

namespace Bittrex.Net.Sockets
{
    public class HttpClientWithUserAgent : IHttpClient
    {
        private readonly IHttpClient httpClient;
        private IConnection connection;

        public HttpClientWithUserAgent()
        {
            httpClient = new DefaultHttpClient();
        }

        public Task<IResponse> Get(string url, Action<IRequest> prepareRequest, bool isLongRunning)
        {
            return httpClient.Get(url, PrepareRequest, isLongRunning);
        }

        public void Initialize(IConnection con)
        {
            connection = con;
            httpClient.Initialize(con);
        }

        public Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData, bool isLongRunning)
        {
            return httpClient.Post(url, PrepareRequest, isLongRunning);
        }

        private void PrepareRequest(IRequest request)
        {
            if(connection.Headers.ContainsKey("User-Agent"))
                request.UserAgent = connection.Headers["User-Agent"];
            request.SetRequestHeaders(new Dictionary<string, string>());            
        }
    }
}
