using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.Extensions.Logging;

namespace Examples
{
    class Program
    {
        static async void Main(string[] args)
        {
            BittrexClient.SetDefaultOptions(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("APIKEY", "APISECRET"),
                LogLevel = LogLevel.Information,
                LogWriters = new List<ILogger>() { new ConsoleLogger() }
            });

            var client = new BittrexClient();
            
            // public
            var markets = await client.GetSymbolsAsync();
            var currencies = await client.GetCurrenciesAsync();
            var price = await client.GetTickerAsync("BTC-ETH");
            var marketSummary = await client.GetSymbolSummaryAsync("BTC-ETH");
            var marketSummaries = await client.GetSymbolSummariesAsync();
            var orderbook = await client.GetOrderBookAsync("BTC-ETH");
            var marketHistory = await client.GetTradeHistoryAsync("BTC-ETH");

            // private
            // Commented to prevent accidental order placement
            //var placedOrder = 
            //    client.PlaceOrder("BTC-USDT", OrderSide.Sell, OrderTypeV3.Limit, TimeInForce.GoodTillCancelled, 1, 1);
            //var orderInfo = client.GetOrder(placedOrder.Data.Id);
            //var canceledOrder = client.CancelOrder(placedOrder.Data.Id);
            var openOrders = await client.GetOpenOrdersAsync("BTC-USDT");
            var orderTrades = await client.GetUserTradesAsync("BTC-NEO");

            var balance = await client.GetBalanceAsync("NEO");
            var balances = await client.GetBalancesAsync();
            var depositAddress = await client.GetDepositAddressAsync("BTC");
            var withdraw = await client.WithdrawAsync("TEST", 1, "TEST", "TEST");
            var withdrawHistory = await client.GetClosedWithdrawalsAsync();
            var depositHistory = await client.GetClosedDepositsAsync();
            

            // Websocket
            var socketClient = new BittrexSocketClient();
            var subscription = socketClient.SubscribeToSymbolTickerUpdatesAsync("ETH-BTC", ticker =>
            {
                Console.WriteLine($"ETH-BTC: {ticker.Data.LastTradeRate}");
            });

            var subscription2 = socketClient.SubscribeToOrderBookUpdatesAsync("ETH-BTC", 25, state =>
            {
                // Order book update
            });

            var subscription3 = socketClient.SubscribeToBalanceUpdatesAsync(data =>
            {
                // Balance update
            });

            Console.ReadLine();
            await socketClient.UnsubscribeAllAsync();
        }
    }
}
