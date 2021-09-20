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
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        void SetApiCredentials(string apiKey, string apiSecret);

        /// <summary>
        /// Subscribe to heartbeat updates
        /// </summary>
        /// <param name="onHeartbeat">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatAsync(Action<DataEvent<DateTime>> onHeartbeat);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to kline(candle) updates for a symbol
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">Interval of the candles</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<BittrexKlineUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to all symbol summary updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(Action<DataEvent<BittrexSummariesUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(string symbol,
            Action<DataEvent<BittrexSymbolSummary>> onUpdate);

        /// <summary>
        /// Subscribe to symbol summary updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolSummaryUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexSymbolSummary>> onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth,
            Action<DataEvent<BittrexOrderBookUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to order book updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="depth">The depth of the oder book to receive update for</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<BittrexOrderBookUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to all symbols ticker updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(Action<DataEvent<BittrexTickersUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTick>> onUpdate);

        /// <summary>
        /// Subscribe to symbol ticker updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTick>> onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<BittrexTradesUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to symbol trade updates
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<BittrexTradesUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to order updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(Action<DataEvent<BittrexOrderUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to balance updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<BittrexBalanceUpdate>> onUpdate);

        /// <summary>
        /// Subscribe to deposit updates
        /// </summary>
        /// <param name="onUpdate">Data handler</param>
        /// <returns>Subscription result</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToDepositUpdatesAsync(Action<DataEvent<BittrexDepositUpdate>> onUpdate);
    }
}