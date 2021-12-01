using Bittrex.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Bittrex API. 
    /// </summary>
    public interface IBittrexClient : IRestClient
    {
        IBittrexClientSpotApi SpotApi { get; }
    }
}