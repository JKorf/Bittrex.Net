using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    internal interface ISignalRSocket: IWebsocket
    {
        void SetHub(string name);

        Task<CallResult<T>> InvokeProxy<T>(string call, params object[] pars);
    }
}
