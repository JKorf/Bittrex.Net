using Microsoft.AspNet.SignalR.Client;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    public interface IHubConnection
    {
        CookieContainer Cookies { get; set; }
        string UserAgent { get; set; }

        IHubProxy CreateHubProxy(string hubName);

        ConnectionState State { get; }

        event Action<StateChange> StateChanged;
        event Action Closed;
        event Action<Exception> Error;
        event Action ConnectionSlow;

        Task Start();
        void Stop(TimeSpan timeout);
    }
}
