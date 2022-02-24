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

        private RestApiClientOptions _spotApiOptions = new RestApiClientOptions(BittrexApiAddresses.Default.RestClientAddress)
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
            set => _spotApiOptions = new RestApiClientOptions(_spotApiOptions, value);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BittrexClientOptions() : this(Default)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn">Base the new options on other options</param>
        internal BittrexClientOptions(BittrexClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            _spotApiOptions = new RestApiClientOptions(baseOn.SpotApiOptions, null);
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

        private ApiClientOptions _spotStreamOptions = new ApiClientOptions(BittrexApiAddresses.Default.SocketClientAddress);
        /// <summary>
        /// Spot stream options
        /// </summary>
        public ApiClientOptions SpotStreamOptions
        {
            get => _spotStreamOptions;
            set => _spotStreamOptions = new ApiClientOptions(_spotStreamOptions, value);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BittrexSocketClientOptions() : this(Default)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn">Base the new options on other options</param>
        internal BittrexSocketClientOptions(BittrexSocketClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            _spotStreamOptions = new ApiClientOptions(baseOn.SpotStreamOptions, null);
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
        public IBittrexSocketClient? SocketClient { get; set; }

        /// <summary>
        /// The rest client to use for requesting the initial order book
        /// </summary>
        public IBittrexClient? RestClient { get; set; }

        /// <summary>
        /// The number of entries in the order book, should be one of: 1/25/500
        /// </summary>
        public int? Limit { get; set; }
    }
}
