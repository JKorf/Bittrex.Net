using CryptoExchange.Net.Interfaces;
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
        /// Get the ISpotClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.
        /// </summary>
        /// <returns></returns>
        public ISpotClient ComonSpotClient { get; }
    }
}
