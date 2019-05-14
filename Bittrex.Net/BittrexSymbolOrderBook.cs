using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using OrderBookEntryType = CryptoExchange.Net.Objects.OrderBookEntryType;

namespace Bittrex.Net
{
    public class BittrexSymbolOrderBook: SymbolOrderBook
    {
        private readonly BittrexSocketClient socketClient;

        public BittrexSymbolOrderBook(string symbol, LogVerbosity logVerbosity = LogVerbosity.Info, IEnumerable<TextWriter> logWriters = null) : base("Bittrex", symbol, true, logVerbosity, logWriters)
        {
            socketClient = new BittrexSocketClient();
        }

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
            var updates = new List<ProcessEntry>();
            updates.AddRange(data.Sells.Select(a => new ProcessEntry(OrderBookEntryType.Ask, a)));
            updates.AddRange(data.Buys.Select(b => new ProcessEntry(OrderBookEntryType.Bid, b)));
            UpdateOrderBook(data.Nonce, data.Nonce, updates);
        }

        protected override async Task<CallResult<bool>> DoResync()
        {
            var queryResult = await socketClient.QueryExchangeStateAsync(Symbol).ConfigureAwait(false);
            if (!queryResult.Success)
                return new CallResult<bool>(false, queryResult.Error);
            
            SetInitialOrderBook(queryResult.Data.Nonce, queryResult.Data.Sells, queryResult.Data.Buys);
            return new CallResult<bool>(true, null);
        }

        public override void Dispose()
        {
            processBuffer.Clear();
            asks.Clear();
            bids.Clear();

            socketClient?.Dispose();
        }
    }
}
