using System;
using System.Collections.Generic;
using Bittrex.Net.Interfaces.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bittrex.Net.Objects
{
    /// <summary>
    /// Options for the Bittrex client
    /// </summary>
    public class BittrexClientOptions : BaseRestClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexClientOptions Default { get; set; } = new BittrexClientOptions();

        private RestApiClientOptions _spotApiOptions = new RestApiClientOptions("https://api.bitfinex.com")
        {
            RateLimiters = new List<IRateLimiter>
            {
                new RateLimiter()
                    .AddTotalRateLimit(60, TimeSpan.FromMinutes(1))
            }
        };
        /// <summary>
        /// Options for the spot API
        /// </summary>
        public RestApiClientOptions SpotApiOptions
        {
            get => _spotApiOptions;
            set => _spotApiOptions.Copy(_spotApiOptions, value);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);            
        }

        /// <summary>
        /// Copy the values of the def to the input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="def"></param>
        public new void Copy<T>(T input, T def) where T : BittrexClientOptions
        {
            base.Copy(input, def);

            input.SpotApiOptions = new RestApiClientOptions(def.SpotApiOptions);
        }
    }
    
    /// <summary>
    /// Options for the Bittrex socket client
    /// </summary>
    public class BittrexSocketClientOptions : BaseSocketClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static BittrexSocketClientOptions Default { get; set; } = new BittrexSocketClientOptions()
        {
            SocketSubscriptionsCombineTarget = 10
        };

        private ApiClientOptions _spotStreamOptions = new ApiClientOptions("https://socket-v3.bittrex.com");
        /// <summary>
        /// Spot stream options
        /// </summary>
        public ApiClientOptions SpotStreamOptions
        {
            get => _spotStreamOptions;
            set => _spotStreamOptions.Copy(_spotStreamOptions, value);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public BittrexSocketClientOptions()
        {
            if (Default == null)
                return;

            Copy(this, Default);
        }

        /// <summary>
        /// Copy the values of the def to the input
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="def"></param>
        public new void Copy<T>(T input, T def) where T : BittrexSocketClientOptions
        {
            base.Copy(input, def);

            input.SpotStreamOptions = new ApiClientOptions(def.SpotStreamOptions);
        }
    }

    /// <summary>
    /// Options for the Bittrex symbol order book
    /// </summary>
    public class BittrexOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IBittrexSocketClient? SocketClient { get; }

        /// <summary>
        /// The rest client to use for requesting the initial order book
        /// </summary>
        public IBittrexClient? RestClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="socketClient">The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.</param>
        /// <param name="restClient">The client to use for the initial order book request.</param>
        public BittrexOrderBookOptions(IBittrexSocketClient? socketClient = null, IBittrexClient? restClient = null)
        {
            SocketClient = socketClient;
            RestClient = restClient;
        }
    }
}
