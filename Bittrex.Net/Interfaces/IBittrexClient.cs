using Bittrex.Net.Objects;
using System;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;

namespace Bittrex.Net.Interfaces
{
    public interface IBittrexClient: IExchangeClient
    {
        CallResult<BittrexMarket[]> GetMarkets();
        Task<CallResult<BittrexMarket[]>> GetMarketsAsync();

        CallResult<BittrexCurrency[]> GetCurrencies();
        Task<CallResult<BittrexCurrency[]>> GetCurrenciesAsync();

        CallResult<BittrexPrice> GetTicker(string market);
        Task<CallResult<BittrexPrice>> GetTickerAsync(string market);

        CallResult<BittrexMarketSummary> GetMarketSummary(string market);
        Task<CallResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market);

        CallResult<BittrexMarketSummary[]> GetMarketSummaries();
        Task<CallResult<BittrexMarketSummary[]>> GetMarketSummariesAsync();

        CallResult<BittrexOrderBook> GetOrderBook(string market);
        Task<CallResult<BittrexOrderBook>> GetOrderBookAsync(string market);

        CallResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market);
        Task<CallResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market);

        CallResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market);
        Task<CallResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market);

        CallResult<BittrexMarketHistory[]> GetMarketHistory(string market);
        Task<CallResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market);

        CallResult<BittrexCandle[]> GetCandles(string market, TickInterval interval);
        Task<CallResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval);

        CallResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval);
        Task<CallResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval);

        CallResult<BittrexGuid> PlaceOrder(OrderSide side, string market, decimal quantity, decimal rate);
        Task<CallResult<BittrexGuid>> PlaceOrderAsync(OrderSide side, string market, decimal quantity, decimal rate);

        CallResult<BittrexOrderResult> PlaceConditionalOrder(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target);
        Task<CallResult<BittrexOrderResult>> PlaceConditionalOrderAsync(OrderSide side, TimeInEffect timeInEffect, string market, decimal quantity, decimal rate, ConditionType conditionType, decimal target);

        CallResult<object> CancelOrder(Guid guid);
        Task<CallResult<object>> CancelOrderAsync(Guid guid);

        CallResult<BittrexOpenOrdersOrder[]> GetOpenOrders(string market = null);
        Task<CallResult<BittrexOpenOrdersOrder[]>> GetOpenOrdersAsync(string market = null);

        CallResult<BittrexBalance> GetBalance(string currency);
        Task<CallResult<BittrexBalance>> GetBalanceAsync(string currency);

        CallResult<BittrexBalance[]> GetBalances();
        Task<CallResult<BittrexBalance[]>> GetBalancesAsync();

        CallResult<BittrexDepositAddress> GetDepositAddress(string currency);
        Task<CallResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency);

        CallResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null);
        Task<CallResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null);

        CallResult<BittrexAccountOrder> GetOrder(Guid guid);
        Task<CallResult<BittrexAccountOrder>> GetOrderAsync(Guid guid);

        CallResult<BittrexOrderHistoryOrder[]> GetOrderHistory(string market = null);
        Task<CallResult<BittrexOrderHistoryOrder[]>> GetOrderHistoryAsync(string market = null);

        CallResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null);
        Task<CallResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null);

        CallResult<BittrexDeposit[]> GetDepositHistory(string currency = null);
        Task<CallResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null);
    }
}
