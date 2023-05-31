using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Objects.Options
{
    /// <summary>
    /// Options for the BittrexRestClient
    /// </summary>
    public class BittrexRestOptions : RestExchangeOptions<BittrexEnvironment>
    {
        /// <summary>
        /// Default options for the rest client
        /// </summary>
        public static BittrexRestOptions Default { get; set; } = new BittrexRestOptions()
        {
            Environment = BittrexEnvironment.Live
        };

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions
        {
            RateLimiters = new List<IRateLimiter>
            {
                new RateLimiter()
                    .AddTotalRateLimit(60, TimeSpan.FromMinutes(1))
            }
        };

        internal BittrexRestOptions Copy()
        {
            var options = Copy<BittrexRestOptions>();
            options.SpotOptions = SpotOptions.Copy<RestApiOptions>();
            return options;
        }
    }
}
