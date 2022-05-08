using System;
using System.Collections.Generic;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Objects.Internal
{
    internal class ConnectionFactory : IWebsocketFactory
    {
        private readonly ApiProxy? _proxy;
        private readonly TimeSpan? _dataTimeout;

        public ConnectionFactory(ApiProxy? proxy, TimeSpan? dataTimeout)
        {
            _proxy = proxy;
            _dataTimeout = dataTimeout;
        }

        public IWebsocket CreateWebsocket(Log log, string url)
        {
            return new BittrexHubConnection(log, _proxy, _dataTimeout, new HubConnection(url));
        }

        public IWebsocket CreateWebsocket(Log log, string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            throw new System.NotImplementedException();
        }
    }
}
