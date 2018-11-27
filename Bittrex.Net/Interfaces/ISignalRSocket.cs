using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    public interface ISignalRSocket: IWebsocket
    {
        void SetHub(string name);

        Task<CallResult<T>> InvokeProxy<T>(string call, params string[] pars);
    }
}
