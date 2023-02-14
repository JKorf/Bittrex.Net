using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Clients.SpotApi;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net.Clients
{
    /// <inheritdoc cref="IBittrexClient" />
    public class BittrexClient : BaseRestClient, IBittrexClient
    {
        #region Api clients
        /// <inheritdoc />
        public IBittrexClientSpotApi SpotApi { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of BittrexClient using the default options
        /// </summary>
        public BittrexClient() : this(BittrexClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of BittrexClient using the provided options
        /// </summary>
        public BittrexClient(BittrexClientOptions options) : base("Bittrex", options)
        {
            SpotApi = AddApiClient(new BittrexClientSpotApi(log, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(BittrexClientOptions options)
        {
            BittrexClientOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
        }
        #endregion
    }
}
