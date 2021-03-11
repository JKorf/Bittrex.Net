using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Sockets;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Interfaces
{
    /// <summary>
    /// Interface for the Bittrex V3 socket client
    /// </summary>
    public interface IBittrexSocketClient: ISocketClient
    {
        /// <summary>
        /// Subscribe to heartbeat updates
        /// </summary>
        /// <param name="onHeartbeat">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DateTime> onHeartbeat);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<BittrexKlineUpdate> onUpdate);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<BittrexKlineUpdate> onUpdate);

        /// <summary>
        /// Subscribe to all symbol summary updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<BittrexSummariesUpdate> onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<BittrexSymbolSummary> onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<BittrexSymbolSummary> onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<BittrexOrderBookUpdate> onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<BittrexOrderBookUpdate> onUpdate);

        /// <summary>
        /// Subscribe to all symbols ticker updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(Action<BittrexTickersUpdate> onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(string symbol,
            Action<BittrexTick> onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(IEnumerable<string> symbols, Action<BittrexTick> onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(string symbol,
            Action<BittrexTradesUpdate> onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTradeUpdatesAsync(IEnumerable<string> symbols, Action<BittrexTradesUpdate> onUpdate);

        /// <summary>
        /// Subscribe to order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<BittrexOrderUpdate> onUpdate);

        /// <summary>
        /// Subscribe to balance updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<BittrexBalanceUpdate> onUpdate);

        /// <summary>
        /// Subscribe to deposit updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<BittrexDepositUpdate> onUpdate);
    }
}