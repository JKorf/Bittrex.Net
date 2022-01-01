using Bittrex.Net.Clients;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bittrex.Net
{
    /// <summary>
    /// Helper functions
    /// </summary>
    public static class BittrexHelpers
    {
        /// <summary>
        /// Validate the string is a valid Bittrex symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static void ValidateBittrexSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");

            if (!Regex.IsMatch(symbolString, "^((([A-Z]|[0-9]){2,})[-](([A-Z]|[0-9]){2,}))$"))
                throw new ArgumentException($"{symbolString} is not a valid Bittrex symbol. Should be [BaseAsset]-[QuoteAsset] e.g. ETH-BTC");
        }

        /// <summary>
        /// Add the IBittrexClient and IBittrexSocketClient to the sevice collection so they can be injected
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="defaultOptionsCallback">Set default options for the client</param>
        /// <returns></returns>
        public static IServiceCollection AddBittrex(this IServiceCollection services, Action<BittrexClientOptions, BittrexSocketClientOptions>? defaultOptionsCallback = null)
        {
            if (defaultOptionsCallback != null)
            {
                var options = new BittrexClientOptions();
                var socketOptions = new BittrexSocketClientOptions();
                defaultOptionsCallback?.Invoke(options, socketOptions);

                BittrexClient.SetDefaultOptions(options);
                BittrexSocketClient.SetDefaultOptions(socketOptions);
            }

            return services.AddTransient<IBittrexClient, BittrexClient>()
                           .AddScoped<IBittrexSocketClient, BittrexSocketClient>();
        }

        /// <summary>
        /// Get the sequence number from the response headers, or null if no sequence number is present
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static long? GetSequence(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            var sequence = headers.SingleOrDefault(r => r.Key == "Sequence").Value?.FirstOrDefault();
            if (sequence != null)
                return long.Parse(sequence);
            return null;
        }
    }
}
