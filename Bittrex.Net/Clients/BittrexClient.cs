using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Clients.Spot;
using Bittrex.Net.Enums;
using Bittrex.Net.Interfaces.Clients.Rest;
using Bittrex.Net.Interfaces.Clients.Spot;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net.Clients.Rest
{
    /// <summary>
    /// Client for the Bittrex V3 API
    /// </summary>
    public class BittrexClient : RestClient, IBittrexClient
    {
        #region Subclients
        public IBittrexClientSpotMarket SpotMarket { get; }
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
            SpotMarket = new BittrexClientSpotMarket(this, options);
        }
        #endregion

        #region methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
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
        protected override void WriteParamBody(IRequest request, Dictionary<string, object> parameters, string contentType)
        {
            if (parameters.Any() && parameters.First().Key == string.Empty)
            {
                var stringData = JsonConvert.SerializeObject(parameters.First().Value);
                request.SetContent(stringData, contentType);
            }
            else
            {
                var stringData = JsonConvert.SerializeObject(parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));
                request.SetContent(stringData, contentType);
            }
        }

        internal Task<WebCallResult<T>> SendRequestAsync<T>(
             RestSubClient subClient,
             Uri uri,
             HttpMethod method,
             CancellationToken cancellationToken,
             Dictionary<string, object>? parameters = null,
             bool signed = false,
             JsonSerializer? deserializer = null) where T : class
                 => base.SendRequestAsync<T>(subClient, uri, method, cancellationToken, parameters, signed, deserializer: deserializer);

        public override void Dispose()
        {
            SpotMarket.Dispose();
            base.Dispose();
        }
        #endregion
    }
}
