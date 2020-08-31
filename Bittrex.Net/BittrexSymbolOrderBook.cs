using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net
{
    /// <summary>
    /// Order book implementation
    /// </summary>
    public class BittrexSymbolOrderBook: SymbolOrderBook
    {
        private readonly IBittrexSocketClientV3 socketClient;
        private readonly IBittrexClientV3 client;
        private readonly int _limit;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="options">Options for the order book</param>
        public BittrexSymbolOrderBook(string symbol, int limit, BittrexOrderBookOptions? options = null) : base(symbol, options ?? new BittrexOrderBookOptions())
        {
            symbol.ValidateBittrexSymbol();
            limit.ValidateIntValues(nameof(limit), 1, 25, 500);
            _limit = limit;
            socketClient = options?.SocketClient ?? new BittrexSocketClientV3(new BittrexSocketClientV3Options()
            {
                LogVerbosity = options?.LogVerbosity ?? LogVerbosity.Info
            });
            client = new BittrexClientV3(new BittrexClientOptions()
            {
                LogVerbosity = options?.LogVerbosity ?? LogVerbosity.Info
            });
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            var subResult = await socketClient.SubscribeToOrderBookUpdatesAsync(Symbol, _limit, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return new CallResult<UpdateSubscription>(null, subResult.Error);

            Status = OrderBookStatus.Syncing;
            // Slight wait to make sure the order book snapshot is from after the start of the stream
            await Task.Delay(300).ConfigureAwait(false);
            var queryResult = await client.GetOrderBookAsync(Symbol, _limit).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                await socketClient.UnsubscribeAll().ConfigureAwait(false);
                return new CallResult<UpdateSubscription>(null, queryResult.Error);
            }

            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bid, queryResult.Data.Ask);
            return new CallResult<UpdateSubscription>(subResult.Data, null);
        }

        private void HandleUpdate(BittrexOrderBookUpdate data)
        {
            UpdateOrderBook(data.Sequence, data.BidDeltas, data.AskDeltas);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResync()
        {
            var queryResult = await client.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(false, queryResult.Error);
            
            SetInitialOrderBook(queryResult.Data.Sequence, queryResult.Data.Bid, queryResult.Data.Ask);
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            processBuffer.Clear();
            asks.Clear();
            bids.Clear();

            socketClient?.Dispose();
        }
    }
}
