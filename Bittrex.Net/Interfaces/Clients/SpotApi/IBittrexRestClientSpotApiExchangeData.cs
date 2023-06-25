using Bittrex.Net.Enums;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects.Models;

namespace Bittrex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Bittrex exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBittrexRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Gets the server time
        /// <para><a href="https://bittrex.github.io/api/v3#operation--ping-get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Time of the server</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about all available symbols
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets-get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbols</returns>
        Task<WebCallResult<IEnumerable<BittrexSymbol>>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about a symbol
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol info</returns>
        Task<WebCallResult<BittrexSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets summaries of all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets-summaries-get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of symbol summaries</returns>
        Task<WebCallResult<IEnumerable<BittrexSymbolSummary>>> GetSymbolSummariesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets summary of a symbol
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--summary-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get info for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol summary</returns>
        Task<WebCallResult<BittrexSymbolSummary>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book of a symbol
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--orderbook-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="limit">The number of results per side for the order book (1, 25 or 500)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol order book</returns>
        Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the trade history of a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--trades-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol trade list</returns>
        Task<WebCallResult<IEnumerable<BittrexTrade>>> GetTradeHistoryAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets the ticker of a symbol
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--ticker-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get ticker for</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol ticker</returns>
        Task<WebCallResult<BittrexTick>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets list of tickers for all symbols. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets-tickers-get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol tickers</returns>
        Task<WebCallResult<IEnumerable<BittrexTick>>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the klines for a symbol. Sequence number of the data available via ResponseHeaders.GetSequence()
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--recent-get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="type">The type of klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol klines</returns>
        Task<WebCallResult<IEnumerable<BittrexKline>>> GetKlinesAsync(string symbol, KlineInterval interval, KlineType? type = null, CancellationToken ct = default);

        /// <summary>
        /// Gets historical klines for a symbol
        /// <para><a href="https://bittrex.github.io/api/v3#operation--markets--marketSymbol--candles--candleType---candleInterval--historical--year---month---day--get" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get klines for</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="year">The year to get klines for</param>
        /// <param name="month">The month to get klines for</param>
        /// <param name="day">The day to get klines for</param>
        /// <param name="type">The type of klines</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Symbol kline</returns>
        Task<WebCallResult<IEnumerable<BittrexKline>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, KlineType? type = null, CancellationToken ct = default);

        /// <summary>
        /// Gets a list of all assets
        /// <para><a href="https://bittrex.github.io/api/v3#operation--currencies-get" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of assets</returns>
        Task<WebCallResult<IEnumerable<BittrexAsset>>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets info on a asset
        /// <para><a href="https://bittrex.github.io/api/v3#operation--currencies--symbol--get" /></para>
        /// </summary>
        /// <param name="asset">The name of the asset</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Asset info</returns>
        Task<WebCallResult<BittrexAsset>> GetAssetAsync(string asset, CancellationToken ct = default);

    }
}
