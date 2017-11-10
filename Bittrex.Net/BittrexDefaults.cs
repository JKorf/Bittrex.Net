using System;
using System.Collections.Generic;
using System.IO;
using Bittrex.Net.Logging;
using Bittrex.Net.RateLimiter;

namespace Bittrex.Net
{
    public static class BittrexDefaults
    {
        internal static string ApiKey { get; private set; }
        internal static string ApiSecret { get; private set; }
        
        internal static LogVerbosity? LogVerbosity { get; private set; }
        internal static TextWriter LogWriter { get; private set; }
        internal static int? MaxCallRetry { get; private set; }
        internal static List<IRateLimiter> RateLimiters { get; } = new List<IRateLimiter>();

        /// <summary>
        /// Sets the API credentials to use. Api keys can be managed at https://bittrex.com/Manage#sectionApi
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret associated with the key</param>
        public static void SetDefaultApiCredentials(string apiKey, string apiSecret)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                throw new ArgumentException("Api key or secret empty");

            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }

        /// <summary>
        /// Sets the default log verbosity for all new clients
        /// </summary>
        /// <param name="logVerbosity">The minimal verbosity to log</param>
        public static void SetDefaultLogVerbosity(LogVerbosity logVerbosity)
        {
            LogVerbosity = logVerbosity;
        }

        /// <summary>
        /// Sets the default log output for all new clients
        /// </summary>
        /// <param name="writer">The output writer</param>
        public static void SetDefaultLogOutput(TextWriter writer)
        {
            LogWriter = writer;
        }

        /// <summary>
        /// Sets the maximum times to retry a call when there is a server error
        /// </summary>
        /// <param name="retry">The maximum retries</param>
        public static void SetDefaultRetries(int retry)
        {
            MaxCallRetry = retry;
        }
        
        /// <summary>
        /// Adds a rate limiter for all new clients.
        /// </summary>
        /// <param name="rateLimiter">The ratelimiter</param>
        public static void AddDefaultRateLimiter(IRateLimiter rateLimiter)
        {
            RateLimiters.Add(rateLimiter);
        }

        /// <summary>
        /// Removes all rate limiters for future clients.
        /// </summary>
        public static void RemoveDefaultRateLimiters()
        {
            RateLimiters.Clear();
        }
    }
}
