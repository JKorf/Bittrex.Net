using Bittrex.Net.Objects.Options;
using CryptoExchange.Net.Interfaces;
using System;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Bittrex order book factory
    /// </summary>
    public interface IBittrexOrderBookFactory
    {
        /// <summary>
        /// Create a SymbolOrderBook
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(string symbol, Action<BittrexOrderBookOptions>? options = null);
    }
}
