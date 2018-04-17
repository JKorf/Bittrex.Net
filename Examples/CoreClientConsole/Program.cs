using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                LogWriters = new List<TextWriter>() { Console.Out }
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
                // Commented to prevent accidental order placement
                //var placedOrder = await client.PlaceOrderAsync(OrderSide.Sell, "BTC-NEO", 1, 1);
                //var orderInfo = await client.GetOrderAsync(placedOrder.Data.Uuid);
                //var canceledOrder = await client.CancelOrderAsync(placedOrder.Data.Uuid);
                var openOrders = await client.GetOpenOrdersAsync("BTC-NEO");
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
            var subscription = await socketClient.SubscribeToMarketSummariesUpdateAsync(summaries =>
            {
                Console.WriteLine($"BTC-ETH: {summaries.SingleOrDefault(s => s.MarketName == "BTC-ETH")?.Last}");
            });

            var subscription2 = await socketClient.SubscribeToExchangeStateUpdatesAsync("BTC-ETH", state =>
            {
            });

            var subscription3 = await socketClient.SubscribeToOrderUpdatesAsync(order =>
            {
            });

            Console.ReadLine();
            socketClient.UnsubscribeAllStreams();
        }
    }
}
