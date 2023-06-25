using Bittrex.Net.Interfaces;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects.Options;
using Bittrex.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bitfinex.Net.SymbolOrderBooks
{
    /// <summary>
    /// Bitfinex order book factory
    /// </summary>
    public class BittrexOrderBookFactory : IBittrexOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BittrexOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ISymbolOrderBook Create(string symbol, Action<BittrexOrderBookOptions>? options = null)
            => new BittrexSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<BittrexSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IBittrexRestClient>(),
                                             _serviceProvider.GetRequiredService<IBittrexSocketClient>());
    }
}
