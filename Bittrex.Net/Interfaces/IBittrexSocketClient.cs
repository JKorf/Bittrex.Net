using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Interface for the Bittrex socket client
    /// </summary>
    public interface IBittrexSocketClient: ISocketClient
    {
        /// <summary>
        /// Gets the current summaries for all symbols
        /// </summary>
        /// <returns>Symbol summaries</returns>
        CallResult<IEnumerable<BittrexStreamSymbolSummary>> GetSymbolSummaries();

        /// <summary>
        /// Gets the current summaries for all symbols
        /// </summary>
        /// <returns>Symbol summaries</returns>
        Task<CallResult<IEnumerable<BittrexStreamSymbolSummary>>> GetSymbolSummariesAsync();

        /// <summary>
        /// Gets the state of a specific symbol
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the symbol to query</param>
        /// <returns>The current exchange state</returns>
        CallResult<BittrexStreamOrderBook> GetOrderBook(string symbol);

        /// <summary>
        /// Gets the state of a specific symbol
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the symbol to query</param>
        /// <returns>The current exchange state</returns>
        Task<CallResult<BittrexStreamOrderBook>> GetOrderBookAsync(string symbol);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific symbol
        /// </summary>
        /// <param name="symbol">The name of the symbol to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, Action<BittrexStreamOrderBookUpdate> onUpdate);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific symbol
        /// </summary>
        /// <param name="symbol">The name of the symbol to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<BittrexStreamOrderBookUpdate> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all symbols
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSymbolSummariesUpdate(Action<IEnumerable<BittrexStreamSymbolSummary>> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all symbols
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummariesUpdateAsync(Action<IEnumerable<BittrexStreamSymbolSummary>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all symbols
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSymbolSummariesLiteUpdate(Action<IEnumerable<BittrexStreamSymbolSummaryLite>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all symbols
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummariesLiteUpdateAsync(Action<IEnumerable<BittrexStreamSymbolSummaryLite>> onUpdate);

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onBalanceUpdate">The update event handler for balance updates</param>
        /// <param name="onOrderUpdate">The update event handler for order updates</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToAccountUpdates(Action<BittrexStreamBalanceData> onBalanceUpdate, Action<BittrexStreamOrderData> onOrderUpdate);

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onBalanceUpdate">The update event handler for balance updates</param>
        /// <param name="onOrderUpdate">The update event handler for order updates</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAccountUpdatesAsync(Action<BittrexStreamBalanceData> onBalanceUpdate, Action<BittrexStreamOrderData> onOrderUpdate);
    }
}