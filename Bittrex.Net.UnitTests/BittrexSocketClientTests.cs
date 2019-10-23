using Bittrex.Net.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Bittrex.Net.UnitTests.TestImplementations;
using CryptoExchange.Net.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bittrex.Net.UnitTests
{
    public class BittrexSocketClientTests
    {
        [TestCase()]
        public void SubscribingToMarketSummaries_Should_ReceiveUpdate()
        {
            // arrange
            var socket = new TestSocket();
            socket.SetProxyResponse(true);
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            List<BittrexStreamSymbolSummary> result = null;
            var subResponse = client.SubscribeToSymbolSummariesUpdate((test) => result = test.ToList());

            var data =
                new BittrexStreamMarketSummaryUpdate()
                {
                    Deltas = new List<BittrexStreamSymbolSummary>
                    {
                        new BittrexStreamSymbolSummary()
                        {
                            Ask = 0.1m,
                            BaseVolume = 0.2m,
                            Bid = 0.3m,
                            Created = new DateTime(2018,1,1),
                            High = 0.4m,
                            Last = 0.5m,
                            Low = 0.6m,
                            Symbol = "TestMarket",
                            OpenBuyOrders = null,
                            OpenSellOrders = null,
                            PrevDay = null,
                            TimeStamp = new DateTime(2018,1,1),
                            Volume = 0.7m
                        }
                    },
                    Nonce = 1
                };
            

            // act
            socket.InvokeMessage(WrapResult("uS", data));

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Deltas.ToList()[0], result[0]));
        }

        [TestCase()]
        public void SubscribingToMarketSummariesLite_Should_ReceiveUpdate()
        {
            // arrange
            var socket = new TestSocket();
            socket.SetProxyResponse(true);
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            List<BittrexStreamSymbolSummaryLite> result = null;
            var subResponse = client.SubscribeToSymbolSummariesLiteUpdate((test) => result = test.ToList());

            var data =
                new BittrexStreamMarketSummariesLite()
                {
                    Deltas = new List<BittrexStreamSymbolSummaryLite>
                    {
                        new BittrexStreamSymbolSummaryLite()
                        {
                            BaseVolume = 0.2m,
                            Last = 0.5m,
                            Symbol = "TestMarket"
                        }
                    }
                };


            // act
            socket.InvokeMessage(WrapResult("uL", data));

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Deltas.ToList()[0], result[0]));
        }

        [TestCase()]
        public void SubscribingToExchangeUpdates_Should_ReceiveUpdate()
        {
            // arrange
            var socket = new TestSocket();
            socket.SetProxyResponse(true);
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket, new BittrexSocketClientOptions()
            {
                LogVerbosity = LogVerbosity.Debug
            });

            BittrexStreamOrderBookUpdate result = null;
            var subResponse = client.SubscribeToOrderBookUpdates("BTC-ETH", (test) => result = test);

            var data =
                new BittrexStreamOrderBookUpdate()
                {
                    Nonce = 1,
                    Symbol = "BTC-ETH",
                    Buys = new List<BittrexStreamOrderBookUpdateEntry> { new BittrexStreamOrderBookUpdateEntry() { Quantity = 0.1m, Price = 0.2m, Type = OrderBookEntryType.NewEntry} },
                    Sells = new List<BittrexStreamOrderBookUpdateEntry> { new BittrexStreamOrderBookUpdateEntry() { Quantity = 0.4m, Price = 0.5m, Type = OrderBookEntryType.RemoveEntry } },
                    Fills = new List<BittrexStreamFill> { new BittrexStreamFill(){ Rate = 0.6m, Quantity = 0.7m, OrderType = OrderSide.Buy, Timestamp = new DateTime(2018, 1, 1)} }
                };


            // act
            socket.InvokeMessage(WrapResult("uE", data));

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data, result, "Buys", "Sells", "Fills"));
            Assert.IsTrue(TestHelpers.AreEqual(data.Buys.ToList()[0], result.Buys.ToList()[0]));
            Assert.IsTrue(TestHelpers.AreEqual(data.Sells.ToList()[0], result.Sells.ToList()[0]));
            Assert.IsTrue(TestHelpers.AreEqual(data.Fills.ToList()[0], result.Fills.ToList()[0]));
        }

        private JObject WrapResult<T>(string method, T data)
        {
            var stringData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            using var compressedStream = new MemoryStream();
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress);
            deflateStream.Write(stringData, 0, stringData.Length);
            deflateStream.Flush();

            var result = new JObject();
            result["M"] = method;
            result["A"] = new JArray(Convert.ToBase64String(compressedStream.ToArray()));
            return result;
        }
    }
}
