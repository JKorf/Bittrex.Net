using System.Collections.Generic;
using System.IO;
using Bittrex.Net.Logging;
using Bittrex.Net.RateLimiter;

namespace Bittrex.Net.Objects
{
    public abstract class BittrexOptions
    {
        /// <summary>
        /// The level the log message schould minimally be to show
        /// </summary>
        public LogVerbosity LogVerbosity { get; set; } = LogVerbosity.Warning;
        /// <summary>
        /// The output writer for log messages
        /// </summary>
        public TextWriter LogWriter { get; set; }

        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; } = -1;
    }

    public class BittrexClientOptions: BittrexOptions
    {
        /// <summary>
        /// The api key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// The api secret
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// The address to use as base for the api calls
        /// </summary>
        public string BaseAddress { get; set; } = "https://www.bittrex.com";
        /// <summary>
        /// The max amount of retries when the server doesn't respond or there is a communication error
        /// </summary>
        public int MaxCallRetry { get; set; } = 2;
        
        /// <summary>
        /// Rate limiters to use
        /// </summary>
        public List<IRateLimiter> RateLimiters { get; set; } = new List<IRateLimiter>();
    }

    public class BittrexSocketClientOptions : BittrexOptions
    {
        /// <summary>
        /// The address used to get CloudFlare clearance
        /// </summary>
        public string CloudFlareAuthenticationAddress { get; set; } = "https://www.bittrex.com/";
        /// <summary>
        /// The base address of the socket connection
        /// </summary>
        public string SocketAddress { get; set; } = "https://socket.bittrex.com/";
    }
}
