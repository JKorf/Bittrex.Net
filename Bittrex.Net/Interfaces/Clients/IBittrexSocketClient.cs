using Bittrex.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients
{
    /// <summary>
    /// Interface for the Bittrex V3 socket client
    /// </summary>
    public interface IBittrexSocketClient : ISocketClient
    {
        IBittrexSocketClientSpotStreams SpotStreams { get; }
    }
}