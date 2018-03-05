using System;
using System.Threading.Tasks;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;

namespace CoreClientConsole
{
    class Program
    {
        // Assumes use of C# 7.1
        public static async Task Main()
        {
            BittrexClient.SetDefaultOptions(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("APIKEY", "APISECRET"),
                LogVerbosity = LogVerbosity.Info,
                LogWriter = Console.Out
            });

            using (var client = new BittrexClient())
            {
                // public
                var markets = await client.GetMarketsAsync();
                var currencies = await client.GetCurrenciesAsync();
                var price = await client.GetTickerAsync("BTC-ETH");
                var marketSummary = await client.GetMarketSummaryAsync("BTC-ETH");
                var marketSummaries = await client.GetMarketSummariesAsync();
                var orderbook = await client.GetOrderBookAsync("BTC-ETH");
                var marketHistory = await client.GetMarketHistoryAsync("BTC-ETH");

                // private
                var placedOrder = await client.PlaceOrderAsync(OrderSide.Sell, "BTC-NEO", 1, 1);
                var openOrders = await client.GetOpenOrdersAsync("BTC-NEO");
                var orderInfo = await client.GetOrderAsync(placedOrder.Data.Uuid);
                var canceledOrder = await client.CancelOrderAsync(placedOrder.Data.Uuid);
                var orderHistory = await client.GetOrderHistoryAsync("BTC-NEO");

                var balance = await client.GetBalanceAsync("NEO");
                var balances = await client.GetBalancesAsync();
                var depositAddress = await client.GetDepositAddressAsync("BTC");
                var withdraw = await client.WithdrawAsync("TEST", 1, "TEST", "TEST");
                var withdrawHistory = await client.GetWithdrawalHistoryAsync();
                var depositHistory = await client.GetDepositHistoryAsync();
            }

            // Websocket
            var socketClient = new BittrexSocketClient();
            var subcription = socketClient.SubscribeToMarketDeltaStream("BTC-ETH", summary =>
            {
                // Handle data
            });
            socketClient.SubscribeToAllMarketDeltaStream(data =>
            {
                // Handle data
            });
            socketClient.SubscribeToExchangeDeltas("BTC-ETH", data =>
            {
                // Handle data
            });

            Console.ReadLine();
            socketClient.UnsubscribeAllStreams();
        }
    }
}
