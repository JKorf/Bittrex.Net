using System;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Clients;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.Models.Socket;
using Bittrex.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace Bittrex.Net.SymbolOrderBooks
{
    /// <summary>
    /// Order book implementation
    /// </summary>
    public class BittrexSymbolOrderBook: SymbolOrderBook
    {
        private readonly IBittrexSocketClient _socketClient;
        private readonly IBittrexRestClient _restClient;
        private readonly int _limit;
        private readonly bool _clientOwner;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BittrexSymbolOrderBook(string symbol, Action<BittrexOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null, null)
        {
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="restClient">Rest client instance</param>
        /// <param name="socketClient">Socket client instance</param>
        public BittrexSymbolOrderBook(string symbol,
            Action<BittrexOrderBookOptions>? optionsDelegate,
            ILogger<BittrexSymbolOrderBook>? logger,
            IBittrexRestClient? restClient,
            IBittrexSocketClient? socketClient) : base(logger, "Bittrex", symbol)
        {
            symbol.ValidateBittrexSymbol();

            var options = BittrexOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _limit = options?.Limit ?? 25;

            _sequencesAreConsecutive = true;
            _strictLevels = true;

            _socketClient = socketClient ?? new BittrexSocketClient();
            _restClient = restClient ?? new BittrexRestClient();
            _clientOwner = restClient == null;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(Symbol, _limit, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return new CallResult<UpdateSubscription>(subResult.Error!);

            if (ct.IsCancellationRequested)
            {
                await subResult.Data.CloseAsync().ConfigureAwait(false);
                return subResult.AsError<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;
            // Slight wait to make sure the order book snapshot is from after the start of the stream
            await Task.Delay(300).ConfigureAwait(false);
            var queryResult = await _restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol, _limit).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                await subResult.Data.CloseAsync().ConfigureAwait(false);
                return new CallResult<UpdateSubscription>(queryResult.Error!);
            }

            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bids, queryResult.Data.Asks);
            return new CallResult<UpdateSubscription>(subResult.Data);
        }

        private void HandleUpdate(DataEvent<BittrexOrderBookUpdate> data)
        {
            UpdateOrderBook(data.Data.Sequence, data.Data.BidDeltas, data.Data.AskDeltas);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            var queryResult = await _restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(queryResult.Error!);
            
            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bids, queryResult.Data.Asks);
            return new CallResult<bool>(true);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
            {
                _socketClient?.Dispose();
                _restClient?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
