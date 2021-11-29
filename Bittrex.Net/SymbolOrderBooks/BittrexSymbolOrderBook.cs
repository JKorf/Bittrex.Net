using System.Threading.Tasks;
using Bittrex.Net.Clients.Rest;
using Bittrex.Net.Clients.Socket;
using Bittrex.Net.Interfaces.Clients.Rest;
using Bittrex.Net.Interfaces.Clients.Socket;
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
        /// <param name="limit">The number of entries in the order book</param>
        /// <param name="options">Options for the order book</param>
        public BittrexSymbolOrderBook(string symbol, int limit, BittrexOrderBookOptions? options = null) : base("Bittrex[Spot]", symbol, options ?? new BittrexOrderBookOptions())
        {
            symbol.ValidateBittrexSymbol();
            limit.ValidateIntValues(nameof(limit), 1, 25, 500);
            _limit = limit;

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
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync()
        {
            var subResult = await socketClient.SpotMarket.SubscribeToOrderBookUpdatesAsync(Symbol, _limit, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return new CallResult<UpdateSubscription>(null, subResult.Error);

            Status = OrderBookStatus.Syncing;
            // Slight wait to make sure the order book snapshot is from after the start of the stream
            await Task.Delay(300).ConfigureAwait(false);
            var queryResult = await restClient.SpotMarket.ExchangeData.GetOrderBookAsync(Symbol, _limit).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                await socketClient.UnsubscribeAllAsync().ConfigureAwait(false);
                return new CallResult<UpdateSubscription>(null, queryResult.Error);
            }

            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bids, queryResult.Data.Asks);
            return new CallResult<UpdateSubscription>(subResult.Data, null);
        }

        private void HandleUpdate(DataEvent<BittrexOrderBookUpdate> data)
        {
            UpdateOrderBook(data.Data.Sequence, data.Data.BidDeltas, data.Data.AskDeltas);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync()
        {
            var queryResult = await restClient.SpotMarket.ExchangeData.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(false, queryResult.Error);
            
            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bids, queryResult.Data.Asks);
            return new CallResult<bool>(true, null);
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
