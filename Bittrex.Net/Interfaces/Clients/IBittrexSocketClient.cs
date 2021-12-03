using Bittrex.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Bittrex websocket API
    /// </summary>
    public interface IBittrexSocketClient : ISocketClient
    {
        /// <summary>
        /// Spot streams
        /// </summary>
        IBittrexSocketClientSpotStreams SpotStreams { get; }
    }
}