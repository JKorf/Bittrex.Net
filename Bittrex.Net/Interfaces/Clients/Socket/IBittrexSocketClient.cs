using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects;
using Bittrex.Net.Sockets;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Interfaces.Clients.Socket
{
    /// <summary>
    /// Interface for the Bittrex V3 socket client
    /// </summary>
    public interface IBittrexSocketClient: ISocketClient
    {
        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Subscribe to heartbeat updates
        /// </summary>
        /// <param name="onHeartbeat">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to all symbol summary updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to all symbols ticker updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to deposit updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate, CancellationToken ct = default);
    }
}