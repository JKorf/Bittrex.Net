using Bittrex.Net.Objects;
using Bittrex.Net.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex.Net.Interfaces
{
    public interface IBittrexClient
    {
        int MaxRetries { get; set; }
        IRequestFactory RequestFactory { get; set; }

        void AddRateLimiter(IRateLimiter limiter);
        void RemoveRateLimiters();

        BittrexApiResult<BittrexMarket[]> GetMarkets();
        Task<BittrexApiResult<BittrexMarket[]>> GetMarketsAsync();

        BittrexApiResult<BittrexCurrency[]> GetCurrencies();
        Task<BittrexApiResult<BittrexCurrency[]>> GetCurrenciesAsync();

        BittrexApiResult<BittrexPrice> GetTicker(string market);
        Task<BittrexApiResult<BittrexPrice>> GetTickerAsync(string market);

        BittrexApiResult<BittrexMarketSummary> GetMarketSummary(string market);
        Task<BittrexApiResult<BittrexMarketSummary>> GetMarketSummaryAsync(string market);

        BittrexApiResult<BittrexMarketSummary[]> GetMarketSummaries();
        Task<BittrexApiResult<BittrexMarketSummary[]>> GetMarketSummariesAsync();

        BittrexApiResult<BittrexOrderBook> GetOrderBook(string market);
        Task<BittrexApiResult<BittrexOrderBook>> GetOrderBookAsync(string market);

        BittrexApiResult<BittrexOrderBookEntry[]> GetBuyOrderBook(string market);
        Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetBuyOrderBookAsync(string market);

        BittrexApiResult<BittrexOrderBookEntry[]> GetSellOrderBook(string market);
        Task<BittrexApiResult<BittrexOrderBookEntry[]>> GetSellOrderBookAsync(string market);

        BittrexApiResult<BittrexMarketHistory[]> GetMarketHistory(string market);
        Task<BittrexApiResult<BittrexMarketHistory[]>> GetMarketHistoryAsync(string market);

        BittrexApiResult<BittrexCandle[]> GetCandles(string market, TickInterval interval);
        Task<BittrexApiResult<BittrexCandle[]>> GetCandlesAsync(string market, TickInterval interval);

        BittrexApiResult<BittrexCandle[]> GetLatestCandle(string market, TickInterval interval);
        Task<BittrexApiResult<BittrexCandle[]>> GetLatestCandleAsync(string market, TickInterval interval);

        BittrexApiResult<BittrexGuid> PlaceOrder(OrderType type, string market, decimal quantity, decimal rate);
        Task<BittrexApiResult<BittrexGuid>> PlaceOrderAsync(OrderType type, string market, decimal quantity, decimal rate);

        BittrexApiResult<object> CancelOrder(Guid guid);
        Task<BittrexApiResult<object>> CancelOrderAsync(Guid guid);

        BittrexApiResult<BittrexOrder[]> GetOpenOrders(string market = null);
        Task<BittrexApiResult<BittrexOrder[]>> GetOpenOrdersAsync(string market = null);

        BittrexApiResult<BittrexBalance> GetBalance(string currency);
        Task<BittrexApiResult<BittrexBalance>> GetBalanceAsync(string currency);

        BittrexApiResult<BittrexBalance[]> GetBalances();
        Task<BittrexApiResult<BittrexBalance[]>> GetBalancesAsync();

        BittrexApiResult<BittrexDepositAddress> GetDepositAddress(string currency);
        Task<BittrexApiResult<BittrexDepositAddress>> GetDepositAddressAsync(string currency);

        BittrexApiResult<BittrexGuid> Withdraw(string currency, decimal quantity, string address, string paymentId = null);
        Task<BittrexApiResult<BittrexGuid>> WithdrawAsync(string currency, decimal quantity, string address, string paymentId = null);

        BittrexApiResult<BittrexAccountOrder> GetOrder(Guid guid);
        Task<BittrexApiResult<BittrexAccountOrder>> GetOrderAsync(Guid guid);

        BittrexApiResult<BittrexOrder[]> GetOrderHistory(string market = null);
        Task<BittrexApiResult<BittrexOrder[]>> GetOrderHistoryAsync(string market = null);

        BittrexApiResult<BittrexWithdrawal[]> GetWithdrawalHistory(string currency = null);
        Task<BittrexApiResult<BittrexWithdrawal[]>> GetWithdrawalHistoryAsync(string currency = null);
        
        BittrexApiResult<BittrexDeposit[]> GetDepositHistory(string currency = null);
        Task<BittrexApiResult<BittrexDeposit[]>> GetDepositHistoryAsync(string currency = null);
    }
}
