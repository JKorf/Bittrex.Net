using Bittrex.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Bittrex Rest API. 
    /// </summary>
    public interface IBittrexClient : IRestClient
    {
        /// <summary>
        /// Spot API
        /// </summary>
        IBittrexClientSpotApi SpotApi { get; }
    }
}