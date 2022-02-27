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
            SpotApi = AddApiClient(new BittrexClientSpotApi(log, this, options));
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
        protected override Error ParseErrorResponse(JToken data)
        {
            if (data["code"] == null)
                return new UnknownError("Unknown response from server", data);

            var info = (string)data["code"]!;
            if (data["detail"] != null)
                info += "; Details: " + (string)data["detail"]!;
            if (data["data"] != null)
                info += "; Data: " + data["data"];

            return new ServerError(info);
        }

        /// <inheritdoc />
        protected override void WriteParamBody(IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            if (parameters.Any() && parameters.First().Key == string.Empty)
            {
                var stringData = JsonConvert.SerializeObject(parameters.First().Value);
                request.SetContent(stringData, contentType);
            }
            else
            {
                var stringData = JsonConvert.SerializeObject(parameters);
                request.SetContent(stringData, contentType);
            }
        }

        internal Task<WebCallResult<T>> SendRequestAsync<T>(
             RestApiClient apiClient,
             Uri uri,
             HttpMethod method,
             CancellationToken cancellationToken,
             Dictionary<string, object>? parameters = null,
             bool signed = false,
             JsonSerializer? deserializer = null,
             bool ignoreRateLimit = false) where T : class
                 => base.SendRequestAsync<T>(apiClient, uri, method, cancellationToken, parameters, signed, deserializer: deserializer, ignoreRatelimit: ignoreRateLimit);

        #endregion
    }
}
