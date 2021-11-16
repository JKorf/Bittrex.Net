﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Interfaces.Clients.Rest
{
    /// <summary>
    /// Interface for the Bittrex V3 API client
    /// </summary>
    public interface IBittrexClient : IRestClient
    
    {
        /// <summary>
        /// Set the API key and secret. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        IBittrexClientAccount Account { get; }
        IBittrexClientExchangeData ExchangeData { get; }
        IBittrexClientTrading Trading { get; }
    }
}