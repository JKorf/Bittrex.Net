using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    public interface IHubConnection
    {
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
