using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Interfaces
{
    public interface IBittrexSocketClient: ISocketClient
    {
        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        CallResult<List<BittrexStreamMarketSummary>> QuerySummaryStates();

        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        Task<CallResult<List<BittrexStreamMarketSummary>>> QuerySummaryStatesAsync();

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="marketName">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        CallResult<BittrexStreamQueryExchangeState> QueryExchangeState(string marketName);

        /// <summary>
        /// Gets the state of a specific market
        /// 500 Buys
        /// 100 Fills
        /// 500 Sells
        /// </summary>
        /// <param name="marketName">The name of the market to query</param>
        /// <returns>The current exchange state</returns>
        Task<CallResult<BittrexStreamQueryExchangeState>> QueryExchangeStateAsync(string marketName);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToExchangeStateUpdates(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Subscribes to order book and trade updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToExchangeStateUpdatesAsync(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToMarketSummariesUpdate(Action<List<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesUpdateAsync(Action<List<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToMarketSummariesLiteUpdate(Action<List<BittrexStreamMarketSummaryLite>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarketSummariesLiteUpdateAsync(Action<List<BittrexStreamMarketSummaryLite>> onUpdate);

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