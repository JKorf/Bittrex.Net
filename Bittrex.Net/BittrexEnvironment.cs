using Bittrex.Net.Objects;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net
{
    /// <summary>
    /// Bittrex environments
    /// </summary>
    public class BittrexEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest client address
        /// </summary>
        public string RestAddress { get; }

        /// <summary>
        /// Socket client address
        /// </summary>
        public string SocketAddress { get; }

        internal BittrexEnvironment(string name, string restAddress, string socketAddress) :
            base(name)
        {
            RestAddress = restAddress;
            SocketAddress = socketAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static BittrexEnvironment Live { get; }
            = new BittrexEnvironment(TradeEnvironmentNames.Live,
                                     BittrexApiAddresses.Default.RestClientAddress,
                                     BittrexApiAddresses.Default.SocketClientAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="restAddress"></param>
        /// <param name="socketAddress"></param>
        /// <returns></returns>
        public static BittrexEnvironment CreateCustom(
                        string name,
                        string restAddress,
                        string socketAddress)
            => new BittrexEnvironment(name, restAddress, socketAddress);
    }
}
