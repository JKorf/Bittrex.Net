using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
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
                var markets = client.GetMarkets();
                var currencies = client.GetCurrencies();
                var price = client.GetTicker("BTC-ETH");
                var marketSummary = client.GetMarketSummary("BTC-ETH");
                var marketSummaries = client.GetMarketSummaries();
                var orderbook = client.GetOrderBook("BTC-ETH");
                var marketHistory = client.GetMarketHistory("BTC-ETH");

                // private
                // Commented to prevent accidental order placement
                //var placedOrder = client.PlaceOrder(OrderSide.Sell, "BTC-NEO", 1, 1);
                //var orderInfo = client.GetOrder(placedOrder.Data.Uuid);
                //var canceledOrder = client.CancelOrder(placedOrder.Data.Uuid);
                var openOrders = client.GetOpenOrders("BTC-NEO");
                var orderHistory = client.GetOrderHistory("BTC-NEO");

                var balance = client.GetBalance("NEO");
                var balances = client.GetBalances();
                var depositAddress = client.GetDepositAddress("BTC");
                var withdraw = client.Withdraw("TEST", 1, "TEST", "TEST");
                var withdrawHistory = client.GetWithdrawalHistory();
                var depositHistory = client.GetDepositHistory();
            }

            // Websocket
            var socketClient = new BittrexSocketClient();
            var subscription = socketClient.SubscribeToMarketSummariesUpdate(summaries =>
            {
                Console.WriteLine($"BTC-ETH: {summaries.SingleOrDefault(s => s.MarketName == "BTC-ETH")?.Last}");
            });

            var subscription2 = socketClient.SubscribeToExchangeStateUpdates("BTC-ETH", state =>
            {
            });

            var subscription3 = socketClient.SubscribeToOrderUpdates(order =>
            {
            });

            Console.ReadLine();
            socketClient.UnsubscribeAllStreams();

        }
    }
}
