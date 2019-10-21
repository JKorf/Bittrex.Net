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
    /// Client for the Bittrex socket client
    /// </summary>
    public interface IBittrexSocketClient: ISocketClient
    {
        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        CallResult<IEnumerable<BittrexStreamMarketSummary>> QuerySummaryStates();

        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        Task<CallResult<IEnumerable<BittrexStreamMarketSummary>>> QuerySummaryStatesAsync();

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        CallResult<BittrexStreamQueryExchangeState> QueryExchangeState(string symbol);

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="symbol">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        Task<CallResult<BittrexStreamQueryExchangeState>> QueryExchangeStateAsync(string symbol);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="symbol">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToExchangeStateUpdates(string symbol, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="symbol">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToExchangeStateUpdatesAsync(string symbol, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToMarketSummariesUpdate(Action<IEnumerable<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesUpdateAsync(Action<IEnumerable<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToMarketSummariesLiteUpdate(Action<IEnumerable<BittrexStreamMarketSummaryLite>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesLiteUpdateAsync(Action<IEnumerable<BittrexStreamMarketSummaryLite>> onUpdate);

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