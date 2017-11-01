using System;
using Bittrex.Net;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            BittrexDefaults.SetDefaultApiCredentials("APIKEY", "APISECRET");
            BittrexDefaults.SetDefaultLogOutput(Console.Out);
            BittrexDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);

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
                var placedOrder = client.PlaceOrder(OrderType.Sell, "BTC-NEO", 1, 1);
                var openOrders = client.GetOpenOrders("BTC-NEO");
                var orderInfo = client.GetOrder(placedOrder.Result.Uuid);
                var canceledOrder = client.CancelOrder(placedOrder.Result.Uuid);
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
            var subcribtion = socketClient.SubscribeToMarketDeltaStream("BTC-ETH", summary =>
            {
                Console.WriteLine($"BTC-ETH: {summary.Last}");
            });

            Console.ReadLine();
            socketClient.UnsubscribeFromStream(subcribtion.Result);

        }
    }
}
