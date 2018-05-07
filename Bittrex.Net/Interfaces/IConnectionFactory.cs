using CryptoExchange.Net.Logging;

namespace Bittrex.Net.Interfaces
{
    public interface IConnectionFactory
    {
        IHubConnection Create(Log log, string url);
    }
}
