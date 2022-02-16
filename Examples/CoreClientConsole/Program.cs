using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bittrex.Net;
using Bittrex.Net.Clients;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.Extensions.Logging;

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
                LogLevel = LogLevel.Information,
                LogWriters = new List<ILogger>() { new ConsoleLogger() }
            });

            using (var client = new BittrexClient())
            {
                // public
                var markets = await client.SpotApi.ExchangeData.GetSymbolsAsync();
                var currencies = await client.SpotApi.ExchangeData.GetAssetsAsync();
                var price = await client.SpotApi.ExchangeData.GetTickerAsync("BTC-USDT");
                var marketSummary = await client.SpotApi.ExchangeData.GetSymbolSummaryAsync("BTC-USDT");
                var marketSummaries = await client.SpotApi.ExchangeData.GetSymbolSummariesAsync();
                var orderbook = await client.SpotApi.ExchangeData.GetOrderBookAsync("BTC-USDT");
                var marketHistory = await client.SpotApi.ExchangeData.GetTradeHistoryAsync("BTC-USDT");

                // private
                // Commented to prevent accidental order placement
                //var placedOrder = 
                //    client.PlaceOrderAsync("BTC-USDT", OrderSide.Sell, OrderTypeV3.Limit, TimeInForce.GoodTillCancelled, 1, 1);
                //var orderInfo = client.GetOrderAsync(placedOrder.Data.Id);
                //var canceledOrder = client.CancelOrderAsync(placedOrder.Data.Id);
                var openOrders = await client.SpotApi.Trading.GetOpenOrdersAsync("BTC-USDT");
                var orderHistory = await client.SpotApi.Trading.GetClosedOrdersAsync("BTC-USDT");

                var balance = await client.SpotApi.Account.GetBalanceAsync("BTC");
                var balances = await client.SpotApi.Account.GetBalancesAsync();
                var depositAddress = await client.SpotApi.Account.GetDepositAddressAsync("BTC");
                var withdraw = await client.SpotApi.Account.WithdrawAsync("TEST", 1, "TEST", "TEST");
                var withdrawHistory = await client.SpotApi.Account.GetClosedWithdrawalsAsync();
                var depositHistory = await client.SpotApi.Account.GetClosedDepositsAsync();
            }

            // Websocket
            var socketClient = new BittrexSocketClient();
            var subscription = socketClient.SpotStreams.SubscribeToTickerUpdatesAsync("ETH-BTC", ticker =>
            {
                Console.WriteLine($"ETH-BTC: {ticker.Data.LastPrice}");
            });

            var subscription2 = socketClient.SpotStreams.SubscribeToOrderBookUpdatesAsync("ETH-BTC", 25, state =>
            {
                // Order book update
            });

            var subscription3 = socketClient.SpotStreams.SubscribeToBalanceUpdatesAsync(data =>
            {
                // Balance update
            });
            Console.ReadLine();
            await socketClient.UnsubscribeAllAsync();
        }
    }
}
