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
                var markets = await client.GetSymbolsAsync();
                var currencies = await client.GetCurrenciesAsync();
                var price = await client.GetTickerAsync("BTC-USDT");
                var marketSummary = await client.GetSymbolSummaryAsync("BTC-USDT");
                var marketSummaries = await client.GetSymbolSummariesAsync();
                var orderbook = await client.GetOrderBookAsync("BTC-USDT");
                var marketHistory = await client.GetSymbolTradesAsync("BTC-USDT");

                // private
                // Commented to prevent accidental order placement
                //var placedOrder = 
                //    client.PlaceOrderAsync("BTC-USDT", OrderSide.Sell, OrderTypeV3.Limit, TimeInForce.GoodTillCancelled, 1, 1);
                //var orderInfo = client.GetOrderAsync(placedOrder.Data.Id);
                //var canceledOrder = client.CancelOrderAsync(placedOrder.Data.Id);
                var openOrders = await client.GetOpenOrdersAsync("BTC-USDT");
                var orderHistory = await client.GetOrderHistoryAsync("BTC-USDT");

                var balance = await client.GetBalanceAsync("BTC");
                var balances = await client.GetBalancesAsync();
                var depositAddress = await client.GetDepositAddressAsync("BTC");
                var withdraw = await client.WithdrawAsync("TEST", 1, "TEST", "TEST");
                var withdrawHistory = await client.GetWithdrawalHistoryAsync();
                var depositHistory = await client.GetDepositHistoryAsync();
            }

            // Websocket
            var socketClient = new BittrexSocketClient();
            var subscription = await socketClient.SubscribeToSymbolSummariesUpdateAsync(summaries =>
            {
                Console.WriteLine($"BTC-USDT: {summaries.SingleOrDefault(s => s.Symbol == "BTC-USDT")?.Last}");
            });

            var subscription2 = await socketClient.SubscribeToOrderBookUpdatesAsync("BTC-ETH", state =>
            {
            });

            var subscription3 = await socketClient.SubscribeToAccountUpdatesAsync(data =>
            {
                // Balance update
            }, data =>
            {
                // Order update
            });

            Console.ReadLine();
            await socketClient.UnsubscribeAll();
        }
    }
}
