using Bittrex.Net.Interfaces.Clients.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Interfaces.Clients.Spot
{
    public interface IBittrexClientSpotMarket: IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        IBittrexClientSpotMarketAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        IBittrexClientSpotMarketExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        IBittrexClientSpotMarketTrading Trading { get; }
    }
}
