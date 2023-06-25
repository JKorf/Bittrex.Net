using Bittrex.Net.Clients.SpotApi;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Bittrex.Net.Clients
{
    /// <inheritdoc cref="IBittrexRestClient" />
    public class BittrexRestClient : BaseRestClient, IBittrexRestClient
    {
        #region Api clients
        /// <inheritdoc />
        public IBittrexRestClientSpotApi SpotApi { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of the BittrexRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BittrexRestClient(Action<BittrexRestOptions> optionsDelegate) : this(null, null, optionsDelegate)
        {
        }

        /// <summary>
        /// Create a new instance of the BittrexRestClient using default options
        /// </summary>
        public BittrexRestClient(ILoggerFactory? loggerFactory = null, HttpClient? httpClient = null) : this(httpClient, loggerFactory, null)
        {
        }

        /// <summary>
        /// Create a new instance of the BittrexRestClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public BittrexRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, Action<BittrexRestOptions>? optionsDelegate = null) 
            : base(loggerFactory, "Bittrex")
        {
            var options = BittrexRestOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new BittrexRestClientSpotApi(_logger, httpClient, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BittrexRestOptions> optionsDelegate)
        {
            var options = BittrexRestOptions.Default.Copy();
            optionsDelegate(options);
            BittrexRestOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
        }
        #endregion
    }
}
