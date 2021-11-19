using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces.Clients.Rest
{
    /// <summary>
    /// Client for accessing the Bittrex API. 
    /// </summary>
    public interface IBittrexClient : IRestClient    
    {
        /// <summary>
        /// Set the API key and secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        IBittrexClientAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        IBittrexClientExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        IBittrexClientTrading Trading { get; }
    }
}