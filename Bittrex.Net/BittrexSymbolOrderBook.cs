using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using OrderBookEntryType = CryptoExchange.Net.Objects.OrderBookEntryType;

namespace Bittrex.Net
{
    /// <summary>
    /// Order book implementation
    /// </summary>
    public class BittrexSymbolOrderBook: SymbolOrderBook
    {
        private readonly IBittrexSocketClient socketClient;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="options">Options for the order book</param>
        public BittrexSymbolOrderBook(string symbol, BittrexOrderBookOptions? options = null) : base(symbol, options ?? new BittrexOrderBookOptions())
        {
            symbol.ValidateBittrexSymbol();
            socketClient = options?.SocketClient ?? new BittrexSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            var subResult = await socketClient.SubscribeToExchangeStateUpdatesAsync(Symbol, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return new CallResult<UpdateSubscription>(null, subResult.Error);

            Status = OrderBookStatus.Syncing;
            var queryResult = await socketClient.QueryExchangeStateAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                await socketClient.UnsubscribeAll().ConfigureAwait(false);
                return new CallResult<UpdateSubscription>(null, queryResult.Error);
            }

            SetInitialOrderBook(queryResult.Data.Nonce, queryResult.Data.Sells, queryResult.Data.Buys);
            return new CallResult<UpdateSubscription>(subResult.Data, null);
        }

        private void HandleUpdate(BittrexStreamUpdateExchangeState data)
        {
            UpdateOrderBook(data.Nonce, data.Nonce, data.Sells, data.Buys);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResync()
        {
            var queryResult = await socketClient.QueryExchangeStateAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(false, queryResult.Error);
            
            SetInitialOrderBook(queryResult.Data.Nonce, queryResult.Data.Sells, queryResult.Data.Buys);
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
