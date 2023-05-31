using System;
using CryptoExchange.Net;
using Microsoft.Extensions.Logging;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Clients.SpotApi;
using CryptoExchange.Net.Authentication;
using Bittrex.Net.Objects.Options;

namespace Bittrex.Net.Clients
{
    /// <inheritdoc cref="IBittrexSocketClient" />
    public class BittrexSocketClient : BaseSocketClient, IBittrexSocketClient
    {
        #region fields

        #endregion

        #region Api clients

        /// <inheritdoc />
        public IBittrexSocketClientSpotApi SpotApi { get; }

        #endregion

        #region ctor

        /// <summary>
        /// Create a new instance of the BittrexSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        public BittrexSocketClient(ILoggerFactory? loggerFactory = null) : this((x) => { }, loggerFactory)
        {
        }

        /// <summary>
        /// Create a new instance of the BittrexSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BittrexSocketClient(Action<BittrexSocketOptions> optionsDelegate) : this(optionsDelegate, null)
        {
        }

        /// <summary>
        /// Create a new instance of the BittrexSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BittrexSocketClient(Action<BittrexSocketOptions> optionsDelegate, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Bittrex")
        {
            var options = BittrexSocketOptions.Default.Copy();
            optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new BittrexSocketClientSpotApi(_logger, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BittrexSocketOptions> optionsDelegate)
        {
            var options = BittrexSocketOptions.Default.Copy();
            optionsDelegate(options);
            BittrexSocketOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
