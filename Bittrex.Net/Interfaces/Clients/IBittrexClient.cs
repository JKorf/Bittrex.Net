using Bittrex.Net.Interfaces.Clients.Spot;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients.Rest
{
    /// <summary>
    /// Client for accessing the Bittrex API. 
    /// </summary>
    public interface IBittrexClient : IRestClient    
    {
        IBittrexClientSpotMarket SpotApi { get; }
    }
}