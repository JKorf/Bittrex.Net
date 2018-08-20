using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiter;
using Microsoft.AspNet.SignalR.Client;

namespace Bittrex.Net.Interfaces
{
    public interface IBittrexSocketClient
    {
        IConnectionFactory ConnectionFactory { get; set; }
        IRequestFactory RequestFactory { get; set; }

        /// <summary>
        /// Connection to the server is lost
        /// </summary>
        event Action ConnectionLost;

        /// <summary>
        /// Connection to the server is restored
        /// </summary>
        event Action ConnectionRestored;

        /// <summary>
        /// Socket opened the connection to the bittrex server event
        /// </summary>
        event Action Opened;

        /// <summary>
        /// Socket connection closed event
        /// </summary>
        event Action Closed;

        /// <summary>
        /// Socket state changed event
        /// </summary>
        event Action<StateChange> StateChanged;

        /// <summary>
        /// Socket error event. Note that this is only for errors thrown by the socket, not for errors in specific calls/events
        /// </summary>
        event Action<Exception> Error;

        /// <summary>
        /// Socket connection slow event. Might indicate a lost connection
        /// </summary>
        event Action Slow;

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.QuerySummaryStatesAsync"/> method
        /// </summary>
        CallResult<List<BittrexStreamMarketSummary>> QuerySummaryStates();

        /// <summary>
        /// Gets the current summaries for all markets
        /// </summary>
        /// <returns>Market summaries</returns>
        Task<CallResult<List<BittrexStreamMarketSummary>>> QuerySummaryStatesAsync();

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.QueryExchangeStateAsync"/> method
        /// </summary>
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
        /// Synchronized version of the <see cref="BittrexSocketClient.SubscribeToExchangeStateUpdatesAsync"/> method
        /// </summary>
        CallResult<int> SubscribeToExchangeStateUpdates(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Subscribes to orderbook and trade updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        Task<CallResult<int>> SubscribeToExchangeStateUpdatesAsync(string marketName, Action<BittrexStreamUpdateExchangeState> onUpdate);

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.SubscribeToMarketSummariesUpdateAsync"/> method
        /// </summary>
        CallResult<int> SubscribeToMarketSummariesUpdate(Action<List<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Subscribes to updates of summaries for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        Task<CallResult<int>> SubscribeToMarketSummariesUpdateAsync(Action<List<BittrexStreamMarketSummary>> onUpdate);

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.SubscribeToMarketSummariesLiteUpdateAsync"/> method
        /// </summary>
        CallResult<int> SubscribeToMarketSummariesLiteUpdate(Action<List<BittrexStreamMarketSummaryLite>> onUpdate);

        /// <summary>
        /// Subscribes to lite summary updates for all markets
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        Task<CallResult<int>> SubscribeToMarketSummariesLiteUpdateAsync(Action<List<BittrexStreamMarketSummaryLite>> onUpdate);

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.SubscribeToBalanceUpdatesAsync"/> method
        /// </summary>
        CallResult<int> SubscribeToBalanceUpdates(Action<BittrexStreamBalance> onUpdate);

        /// <summary>
        /// Subscribes to balance updates
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        Task<CallResult<int>> SubscribeToBalanceUpdatesAsync(Action<BittrexStreamBalance> onUpdate);

        /// <summary>
        /// Synchronized version of the <see cref="BittrexSocketClient.SubscribeToOrderUpdatesAsync"/> method
        /// </summary>
        CallResult<int> SubscribeToOrderUpdates(Action<BittrexStreamOrderData> onUpdate);

        /// <summary>
        /// Subscribes to order updates
        /// </summary>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns>ApiResult whether subscription was successful. The Result property contains the Stream Id which can be used to unsubscribe the stream again</returns>
        Task<CallResult<int>> SubscribeToOrderUpdatesAsync(Action<BittrexStreamOrderData> onUpdate);

        /// <summary>
        /// Unsubsribe from updates of a specific stream using the stream id acquired when subscribing
        /// </summary>
        /// <param name="streamId">The stream id of the stream to unsubscribe</param>
        void UnsubscribeFromStream(int streamId);

        /// <summary>
        /// Unsubscribes all streams on this client
        /// </summary>
        void UnsubscribeAllStreams();

        void Dispose();
        void AddRateLimiter(IRateLimiter limiter);
        void RemoveRateLimiters();
        CallResult<long> Ping();
        Task<CallResult<long>> PingAsync();
    }
}