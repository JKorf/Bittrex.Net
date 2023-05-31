using CryptoExchange.Net.Objects.Options;
using System;

namespace Bittrex.Net.Objects.Options
{
    /// <summary>
    /// Options for the BittrexSocketClient
    /// </summary>
    public class BittrexSocketOptions : SocketExchangeOptions<BittrexEnvironment>
    {
        /// <summary>
        /// Default options for the socket client
        /// </summary>
        public static BittrexSocketOptions Default { get; set; } = new BittrexSocketOptions()
        {
            Environment = BittrexEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10,
            SocketNoDataTimeout = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public SocketApiOptions SpotOptions { get; private set; } = new SocketApiOptions();

        internal BittrexSocketOptions Copy()
        {
            var options = Copy<BittrexSocketOptions>();
            options.SpotOptions = SpotOptions.Copy<SocketApiOptions>();
            return options;
        }
    }
}
