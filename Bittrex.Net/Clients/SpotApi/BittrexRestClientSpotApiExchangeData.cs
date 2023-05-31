using Bittrex.Net.Converters;
using Bittrex.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Interfaces.Clients.SpotApi;

namespace Bittrex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class BittrexRestClientSpotApiExchangeData : IBittrexRestClientSpotApiExchangeData
    {
        private readonly BittrexRestClientSpotApi _baseClient;

        internal BittrexRestClientSpotApiExchangeData(BittrexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexSymbol>>(_baseClient.GetUrl("markets"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestAsync<BittrexServerTime>(_baseClient.GetUrl("ping"), HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await _baseClient.SendRequestAsync<BittrexSymbol>(_baseClient.GetUrl("markets/" + symbol), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexSymbolSummary>>> GetSymbolSummariesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexSymbolSummary>>(_baseClient.GetUrl("markets/summaries"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexSymbolSummary>> GetSymbolSummaryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await _baseClient.SendRequestAsync<BittrexSymbolSummary>(_baseClient.GetUrl($"markets/{symbol}/summary"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            limit?.ValidateIntValues(nameof(limit), 1, 25, 500);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("depth", limit?.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestAsync<BittrexOrderBook>(_baseClient.GetUrl($"markets/{symbol}/orderbook"), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (result.Data != null)
                result.Data.Sequence = result.ResponseHeaders!.GetSequence() ?? 0;
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTrade>>> GetTradeHistoryAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexTrade>>(_baseClient.GetUrl($"markets/{symbol}/trades"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();
            var result = await _baseClient.SendRequestAsync<BittrexTick>(_baseClient.GetUrl($"markets/{symbol}/ticker"), HttpMethod.Get, ct).ConfigureAwait(false);
            if (result.Success && string.IsNullOrEmpty(result.Data.Symbol))
                result.Data.Symbol = symbol;
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexTick>>> GetTickersAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexTick>>(_baseClient.GetUrl("markets/tickers"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetKlinesAsync(string symbol, KlineInterval interval, KlineType? type = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexKline>>(_baseClient.GetUrl($"markets/{symbol}/candles{(type.HasValue ? "/" + type.ToString().ToUpperInvariant() : "")}/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/recent"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexKline>>> GetHistoricalKlinesAsync(string symbol, KlineInterval interval, int year, int? month = null, int? day = null, KlineType? type = null, CancellationToken ct = default)
        {
            symbol.ValidateBittrexSymbol();

            if (interval == KlineInterval.OneDay && month.HasValue)
                throw new ArgumentException("Can't specify month value when using day interval");

            if (interval == KlineInterval.OneHour && day.HasValue)
                throw new ArgumentException("Can't specify day value when using hour interval");

            if (day.HasValue && !month.HasValue)
                throw new ArgumentException("Can't specify day value without month value");

            var url =
                $"markets/{symbol}/candles{(type.HasValue ? "/" + type.ToString().ToUpperInvariant() : "")}/{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}/historical/{year}";
            if (month.HasValue)
                url += "/" + month;
            if (day.HasValue)
                url += "/" + day;

            return await _baseClient.SendRequestAsync<IEnumerable<BittrexKline>>(_baseClient.GetUrl(url), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        #region currencies

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BittrexAsset>>> GetAssetsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestAsync<IEnumerable<BittrexAsset>>(_baseClient.GetUrl("currencies"), HttpMethod.Get, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<BittrexAsset>> GetAssetAsync(string asset, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            return await _baseClient.SendRequestAsync<BittrexAsset>(_baseClient.GetUrl($"currencies/{asset}"), HttpMethod.Get, ct).ConfigureAwait(false);
        }
        #endregion
    }
}
