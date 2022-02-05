using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Clients;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects;
using Bittrex.Net.Objects.Models.Socket;
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
        private readonly IBittrexSocketClient socketClient;
        private readonly IBittrexClient restClient;
        private readonly int _limit;
        private readonly bool _socketOwner;
        private readonly bool _restOwner;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="options">Options for the order book</param>
        public BittrexSymbolOrderBook(string symbol, BittrexOrderBookOptions? options = null) : base("Bittrex", symbol, options ?? new BittrexOrderBookOptions())
        {
            symbol.ValidateBittrexSymbol();
            _limit = options?.Limit ?? 25;

            sequencesAreConsecutive = true;
            strictLevels = true;

            socketClient = options?.SocketClient ?? new BittrexSocketClient(new BittrexSocketClientOptions()
            {
                LogLevel = options?.LogLevel ?? LogLevel.Information
            });
            restClient = options?.RestClient ?? new BittrexClient(new BittrexClientOptions()
            {
                LogLevel = options?.LogLevel ?? LogLevel.Information
            });
            _socketOwner = options?.SocketClient == null;
            _restOwner = options?.RestClient == null;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await socketClient.SpotStreams.SubscribeToOrderBookUpdatesAsync(Symbol, _limit, HandleUpdate).ConfigureAwait(false);
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
            var queryResult = await restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol, _limit).ConfigureAwait(false);
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
            var queryResult = await restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(queryResult.Error!);
            
            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bids, queryResult.Data.Asks);
            return new CallResult<bool>(true);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            processBuffer.Clear();
            asks.Clear();
            bids.Clear();

            if(_restOwner)
                restClient?.Dispose();
            if (_socketOwner)
                socketClient?.Dispose();
        }
    }
}
