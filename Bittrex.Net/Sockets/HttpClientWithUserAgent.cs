using Microsoft.AspNet.SignalR.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Sockets
{
    public class HttpClientWithUserAgent : IHttpClient
    {
        private readonly IHttpClient _httpClient;
        private IConnection _connection;

        public HttpClientWithUserAgent()
        {
            _httpClient = new DefaultHttpClient();
        }

        public Task<IResponse> Get(string url, Action<IRequest> prepareRequest, bool isLongRunning)
        {
            return _httpClient.Get(url, r => PrepareRequest(prepareRequest, r), isLongRunning);
        }

        public void Initialize(IConnection connection)
        {
            this._connection = connection;

            _httpClient.Initialize(connection);
        }

        public Task<IResponse> Post(string url, Action<IRequest> prepareRequest, IDictionary<string, string> postData, bool isLongRunning)
        {
            return _httpClient.Post(url, r => PrepareRequest(prepareRequest, r), isLongRunning);
        }

        private void PrepareRequest(Action<IRequest> prepareRequest, IRequest request)
        {
            if(_connection.Headers.ContainsKey("User-Agent"))
                request.UserAgent = _connection.Headers["User-Agent"];
            request.SetRequestHeaders(new Dictionary<string, string>());            
        }
    }
}
