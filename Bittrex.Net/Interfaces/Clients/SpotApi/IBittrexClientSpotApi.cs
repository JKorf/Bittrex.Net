using CryptoExchange.Net.ExchangeInterfaces;
using System;

namespace Bittrex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Bittrex spot API endpoints
    /// </summary>
    public interface IBittrexClientSpotApi : IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        IBittrexClientSpotApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        IBittrexClientSpotApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        IBittrexClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the IExchangeClient for this client
        /// </summary>
        /// <returns></returns>
        IExchangeClient AsExchangeClient();
    }
}
