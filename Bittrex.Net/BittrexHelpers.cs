using Bittrex.Net.Clients;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using Bitfinex.Net.SymbolOrderBooks;
using Bittrex.Net.Interfaces;

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
        /// <param name="defaultRestOptionsDelegate">Set default options for the rest client</param>
        /// <param name="defaultSocketOptionsDelegate">Set default options for the socket client</param>
        /// <param name="socketClientLifeTime">The lifetime of the IBittrexSocketClient for the service collection. Defaults to Singleton.</param>
        /// <returns></returns>
        public static IServiceCollection AddBittrex(
            this IServiceCollection services,
            Action<BittrexRestOptions>? defaultRestOptionsDelegate = null,
            Action<BittrexSocketOptions>? defaultSocketOptionsDelegate = null,
            ServiceLifetime? socketClientLifeTime = null)
        {
            var restOptions = BittrexRestOptions.Default.Copy();

            if (defaultRestOptionsDelegate != null)
            {
                defaultRestOptionsDelegate(restOptions);
                BittrexRestClient.SetDefaultOptions(defaultRestOptionsDelegate);
            }

            if (defaultSocketOptionsDelegate != null)
                BittrexSocketClient.SetDefaultOptions(defaultSocketOptionsDelegate);

            services.AddHttpClient<IBittrexRestClient, BittrexRestClient>(options =>
            {
                options.Timeout = restOptions.RequestTimeout;
            }).ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                if (restOptions.Proxy != null)
                {
                    handler.Proxy = new WebProxy
                    {
                        Address = new Uri($"{restOptions.Proxy.Host}:{restOptions.Proxy.Port}"),
                        Credentials = restOptions.Proxy.Password == null ? null : new NetworkCredential(restOptions.Proxy.Login, restOptions.Proxy.Password)
                    };
                }
                return handler;
            });

            services.AddSingleton<IBittrexOrderBookFactory, BittrexOrderBookFactory>();
            services.AddTransient<IBittrexRestClient, BittrexRestClient>();
            services.AddTransient(x => x.GetRequiredService<IBittrexRestClient>().SpotApi.CommonSpotClient);
            if (socketClientLifeTime == null)
                services.AddSingleton<IBittrexSocketClient, BittrexSocketClient>();
            else
                services.Add(new ServiceDescriptor(typeof(IBittrexSocketClient), typeof(BittrexSocketClient), socketClientLifeTime.Value));
            return services;
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
