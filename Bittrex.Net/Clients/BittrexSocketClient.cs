using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Interfaces;
using CryptoExchange.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading;
using Bittrex.Net.Objects.Internal;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Interfaces.Clients.SpotApi;
using Bittrex.Net.Clients.SpotApi;
using CryptoExchange.Net.Authentication;

namespace Bittrex.Net.Clients
{
    /// <inheritdoc cref="IBittrexSocketClient" />
    public class BittrexSocketClient : BaseSocketClient, IBittrexSocketClient
    {
        #region fields

        #endregion

        #region Api clients

        /// <inheritdoc />
        public IBittrexSocketClientSpotStreams SpotStreams { get; }

        #endregion

        #region ctor
        /// <summary>
        /// Creates a new socket client using the default options
        /// </summary>
        public BittrexSocketClient() : this(BittrexSocketClientOptions.Default)
        {
        }

        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClient(BittrexSocketClientOptions options) : base("Bittrex", options)
        {
            SpotStreams = AddApiClient(new BittrexSocketClientSpotStreams(log, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(BittrexSocketClientOptions options)
        {
            BittrexSocketClientOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotStreams.SetApiCredentials(credentials);
        }
    }
}
