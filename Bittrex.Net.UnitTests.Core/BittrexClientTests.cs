using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using Bittrex.Net.RateLimiter;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bittrex.Net.UnitTests.Core
{
    [TestFixture()]
    public class BittrexClientTests
    {
        [TestCase()]
        public void GetMarkets_Should_RespondWithMarketsList()
        {
            // arrange
            var expected = new List<BittrexMarket>()
            {
                new BittrexMarket()
                {
                    BaseCurrency = "Test1",
                    BaseCurrencyLong = "TestCurrency1",
                    Created = new DateTime(2017, 1, 1),
                    IsActive = true,
                    MarketCurrency = "MarketTest1",
                    MarketCurrencyLong = "MarketTestCurrency1",
                    MarketName = "Test1-Test1",
                    MinTradeSize = 0.0001
                },
                new BittrexMarket()
                {
                    BaseCurrency = "Test2",
                    BaseCurrencyLong = "TestCurrency2",
                    Created = new DateTime(2016, 1, 1),
                    IsActive = false,
                    MarketCurrency = "MarketTest2",
                    MarketCurrencyLong = "MarketTestCurrency2",
                    MarketName = "Test2-Test2",
                    MinTradeSize = 1
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetMarkets();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetCurrencies_Should_RespondWithCurrenciesList()
        {
            // arrange
            var expected = new List<BittrexCurrency>()
            {
                new BittrexCurrency()
                {
                    BaseAddress = "TestAddress1",
                    IsActive = true,
                    CoinType = "TESTCOIN",
                    Currency = "TTN",
                    CurrencyLong = "TestToken",
                    MinConfirmation = 10,
                    TransactionFee = 0.2
                },
                new BittrexCurrency()
                {
                    BaseAddress = "TestAddress2",
                    IsActive = true,
                    CoinType = "TESTCOIN",
                    Currency = "TTN2",
                    CurrencyLong = "TestToken2",
                    MinConfirmation = 2,
                    TransactionFee = 3
                },
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetCurrencies();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetTicker_Should_RespondWithPrice()
        {
            // arrange
            var expected = new BittrexPrice() { Ask = 0.001, Bid = 0.002, Last = 0.003 };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetTicker("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void GetMarketSummary_Should_RespondWithMarketSummary()
        {
            // arrange
            var expected = new BittrexMarketSummary()
            {
                Ask = 0.001,
                Bid = 0.002,
                Last = 0.003,
                Created = new DateTime(2017, 1, 1),
                MarketName = "TestMarket",
                BaseVolume = 1.1,
                DisplayMarketName = null,
                High = 2.2,
                Low = 3.3,
                OpenBuyOrders = 10,
                OpenSellOrders = 20,
                PrevDay = 4.4,
                TimeStamp = new DateTime(2016, 1, 1),
                Volume = 5.5
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(new List<BittrexMarketSummary>() { expected })));

            // act
            var result = client.GetMarketSummary("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void GetMarketSummaries_Should_RespondWithMarketSummariesList()
        {
            // arrange
            var expected = new List<BittrexMarketSummary>()
            {
                new BittrexMarketSummary()
                {
                    Ask = 0.001,
                    Bid = 0.002,
                    Last = 0.003,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = 1.1,
                    DisplayMarketName = null,
                    High = 2.2,
                    Low = 3.3,
                    OpenBuyOrders = 10,
                    OpenSellOrders = 20,
                    PrevDay = 4.4,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = 5.5
                },
                new BittrexMarketSummary()
                {
                    Ask = 0.006,
                    Bid = 0.007,
                    Last = 0.008,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = 9.9,
                    DisplayMarketName = null,
                    High = 10.10,
                    Low = 11.11,
                    OpenBuyOrders = 30,
                    OpenSellOrders = 40,
                    PrevDay = 12.12,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = 13.13
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetMarketSummaries();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetOrderBook_Should_ReturnOrderBook()
        {
            // arrange
            var expected = new BittrexOrderBook()
            {
                Buy = new List<BittrexOrderBookEntry>()
                {
                    new BittrexOrderBookEntry(){Quantity = 1.1, Rate = 2.2},
                    new BittrexOrderBookEntry(){Quantity = 3.3, Rate = 3.3},
                },
                Sell = new List<BittrexOrderBookEntry>()
                {
                    new BittrexOrderBookEntry(){Quantity = 4.4, Rate = 5.5},
                    new BittrexOrderBookEntry(){Quantity = 6.6, Rate = 7.7},
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result.Buy[0], expected.Buy[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result.Buy[1], expected.Buy[1]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result.Sell[0], expected.Sell[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result.Sell[1], expected.Sell[1]));
        }

        [TestCase()]
        public void GetBuyOrderBook_Should_ReturnBuyOrderBook()
        {
            // arrange

            var expected = new[]
            {
                new BittrexOrderBookEntry() {Quantity = 1.1, Rate = 2.2},
                new BittrexOrderBookEntry() {Quantity = 3.3, Rate = 3.3},
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetBuyOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetSellOrderBook_Should_ReturnSellOrderBook()
        {
            // arrange

            var expected = new[]
            {
                new BittrexOrderBookEntry() {Quantity = 1.1, Rate = 2.2},
                new BittrexOrderBookEntry() {Quantity = 3.3, Rate = 3.3},
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetSellOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetMarketHistory_Should_ReturnMarketHistoryList()
        {
            // arrange
            var expected = new List<BittrexMarketHistory>()
            {
                new BittrexMarketHistory()
                {
                    Quantity = 1.1,
                    OrderType = OrderType.Buy,
                    FillType = FillType.Fill,
                    Id = 1000000000000,
                    Price = 2.2,
                    Timestamp = new DateTime(2017, 1, 1),
                    Total = 3.3
                },
                new BittrexMarketHistory()
                {
                    Quantity = 4.4,
                    OrderType = OrderType.Sell,
                    FillType = FillType.PartialFill,
                    Id = 2000000000000,
                    Price = 5.5,
                    Timestamp = new DateTime(2017, 1, 1),
                    Total = 6.6
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetMarketHistory("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void PlacingOrder_Should_ReturnOrderGuid()
        {
            // arrange
            var expected = new BittrexGuid() { Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301") };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.PlaceOrder(OrderType.Buy, "TestMarket", 1, 1);

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void CancelOrder_Should_ReturnSuccess()
        {
            // arrange
            var expected = new object();
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.CancelOrder(new Guid());

            // assert
            Assert.IsTrue(result.Success);
        }

        [TestCase()]
        public void GetOpenOrders_Should_ReturnOpenOrdersList()
        {
            // arrange
            var expected = new List<BittrexOrder>()
            {
                new BittrexOrder()
                {
                    Uuid = null,
                    Quantity = 1.1,
                    OrderType = OrderTypeExtended.LimitBuy,
                    Price = 2.2,
                    Closed = null,
                    CancelInitiated = false,
                    CommissionPaid = 3.3,
                    Condition = null,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = false,
                    IsConditional = false,
                    Limit = 4.4,
                    Opened = new DateTime(2017, 1,1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = null,
                    QuantityRemaining = 5.5
                },
                new BittrexOrder()
                {
                    Uuid = null,
                    Quantity = 6.6,
                    OrderType = OrderTypeExtended.LimitSell,
                    Price = 7.7,
                    Closed = new DateTime(2017, 1, 1),
                    CancelInitiated = true,
                    CommissionPaid = 8.8,
                    Condition = null,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = true,
                    IsConditional = false,
                    Limit = 9.9,
                    Opened = new DateTime(2017, 1,1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 11.11,
                    QuantityRemaining = 10.1
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetOpenOrders();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetBalance_Should_ReturnBalance()
        {
            // arrange
            var expected = new BittrexBalance()
            {
                Uuid = null,
                Currency = "TestCurrency",
                Available = 1.1,
                Balance = 2.2,
                CryptoAddress = "TestAddress",
                Pending = 3.3,
                Requested = true
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetBalance("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void GetBalances_Should_ReturnBalanceList()
        {
            // arrange
            var expected = new List<BittrexBalance>()
            {
                new BittrexBalance()
                {
                    Uuid = null,
                    Currency = "TestCurrency",
                    Available = 1.1,
                    Balance = 2.2,
                    CryptoAddress = "TestAddress",
                    Pending = 3.3,
                    Requested = true
                },
                new BittrexBalance()
                {
                    Uuid = null,
                    Currency = "TestCurrency",
                    Available = 4.4,
                    Balance = 5.5,
                    CryptoAddress = "TestAddress",
                    Pending = 6.6,
                    Requested = false
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetBalances();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetDepositAddress_Should_ReturnDespositAddress()
        {
            // arrange
            var expected = new BittrexDepositAddress()
            {
                Currency = "TestCurrency",
                Address = "TestAddress"
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetDepositAddress("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void Withdraw_Should_ReturnWithdrawGuid()
        {
            // arrange
            var expected = new BittrexGuid()
            {
                Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.Withdraw("TestCurrency", 1, "TestAddress");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void GetOrder_Should_ReturnOrderInfo()
        {
            // arrange
            var expected = new BittrexAccountOrder()
            {
                Quantity = 1.1,
                Price = 2.2,
                CommissionPaid = 3.3,
                OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                Condition = null,
                CancelInitiated = false,
                Exchange = "MarketExchange",
                PricePerUnit = null,
                QuantityRemaining = 4.4,
                Closed = null,
                ImmediateOrCancel = false,
                ConditionTarget = null,
                IsConditional = false,
                Opened = new DateTime(2017, 1, 1),
                Limit = 5.5,
                AccountId = "TestId",
                CommissionReserved = 6.6,
                CommissionReservedRemaining = 7.7,
                IsOpen = true,
                Reserved = 8.8,
                ReservedRemaining = 9.9,
                Sentinel = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                Type = OrderTypeExtended.LimitBuy
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetOrder(new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"));

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result, expected));
        }

        [TestCase()]
        public void GetOrderHistory_Should_ReturnOrderHistoryList()
        {
            // arrange
            var expected = new List<BittrexOrder>()
            {
                new BittrexOrder()
                {
                    Uuid = null,
                    Quantity = 1.1,
                    OrderType = OrderTypeExtended.LimitBuy,
                    Price = 2.2,
                    Closed = null,
                    CancelInitiated = false,
                    CommissionPaid = 3.3,
                    Condition = null,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = false,
                    IsConditional = false,
                    Limit = 4.4,
                    Opened = new DateTime(2017, 1, 1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = null,
                    QuantityRemaining = 5.5
                },
                new BittrexOrder()
                {
                    Uuid = null,
                    Quantity = 6.6,
                    OrderType = OrderTypeExtended.LimitSell,
                    Price = 7.7,
                    Closed = new DateTime(2017, 1, 1),
                    CancelInitiated = true,
                    CommissionPaid = 8.8,
                    Condition = null,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = true,
                    IsConditional = false,
                    Limit = 9.9,
                    Opened = new DateTime(2017, 1, 1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 11.11,
                    QuantityRemaining = 10.1
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetOrderHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetWithdrawalHistory_Should_ReturnWithdrawalHistoryList()
        {
            // arrange
            var expected = new List<BittrexWithdrawal>()
            {
                new BittrexWithdrawal()
                {
                    Opened = new DateTime(2017, 1, 1),
                    Currency = "TestCurrency",
                    Address = "TestAddress",
                    Amount = 1.1,
                    Authorized = true,
                    Canceled = false,
                    InvalidAddress = false,
                    PaymentUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PendingPayment = false,
                    TransactionCost = 2.2,
                    TransactionId = "TestId"
                },
                new BittrexWithdrawal()
                {
                    Opened = new DateTime(2016, 1, 1),
                    Currency = "TestCurrency",
                    Address = "TestAddress",
                    Amount = 3.3,
                    Authorized = false,
                    Canceled = true,
                    InvalidAddress = true,
                    PaymentUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PendingPayment = true,
                    TransactionCost = 4.4,
                    TransactionId = "TestId"
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetWithdrawalHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void GetDepositHistory_Should_ReturnDepositHistoryList()
        {
            // arrange
            var expected = new List<BittrexDeposit>()
            {
                new BittrexDeposit()
                {
                    Currency = "TestCurrency",
                    Amount = 1.1,
                    TransactionId = "TestId",
                    CryptoAddress = "TestAddress",
                    Id = 1000000000000,
                    Confirmations = 10,
                    LastUpdated = new DateTime(2017, 1, 1)
                },
                new BittrexDeposit()
                {
                    Currency = "TestCurrency",
                    Amount = 2.2,
                    TransactionId = "TestId",
                    CryptoAddress = "TestAddress",
                    Id = 2000000000000,
                    Confirmations = 20,
                    LastUpdated = new DateTime(2016, 1, 1)
                }
            };
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = client.GetDepositHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[0], expected[0]));
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result.Result[1], expected[1]));
        }

        [TestCase()]
        public void FailedResponse_Should_GiveFailedResult()
        {
            // arrange
            var errorMessage = "TestErrorMessage";
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(new BittrexPrice(), false, errorMessage)));

            // act
            var result = client.GetTicker("TestMarket");

            // assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.Error.ErrorMessage);
        }

        [TestCase()]
        public void ReceivingInvalidData_Should_ReturnError()
        {
            // arrange
            var client = PrepareClient("TestErrorNotValidJson");

            // act
            var result = client.GetTicker("TestMarket");

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error.ErrorMessage);
            Assert.IsTrue(result.Error.ErrorMessage.Contains("TestErrorNotValidJson"));
        }

        [TestCase()]
        public void WhenUsingRateLimiterTotalRequests_Should_BeDelayed()
        {
            // arrange
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(new BittrexPrice())));
            client.AddRateLimiter(new RateLimiterTotal(1, TimeSpan.FromSeconds(5)));

            // act
            var sw = Stopwatch.StartNew();
            var result = client.GetTicker("TestMarket");
            result = client.GetTicker("TestMarket");
            result = client.GetTicker("TestMarket");
            sw.Stop();

            // assert
            Assert.IsTrue(sw.ElapsedMilliseconds > 10000);
        }

        [TestCase()]
        public void WhenUsingRateLimiterPerEndpointRequests_Should_BeDelayed()
        {
            // arrange
            var client = PrepareClient(JsonConvert.SerializeObject(WrapInResult(new BittrexPrice())));
            client.AddRateLimiter(new RateLimiterPerEndpoint(1, TimeSpan.FromSeconds(5)));

            // act
            var sw = Stopwatch.StartNew();
            var result = client.GetTicker("TestMarket");
            result = client.GetTicker("TestMarket");
            result = client.GetTicker("TestMarket");
            sw.Stop();

            // assert
            Assert.IsTrue(sw.ElapsedMilliseconds > 10000);
        }

        [TestCase()]
        public void ReceivingErrorStatusCode_Should_NotSuccess()
        {
            // arrange
            var client = PrepareExceptionClient(JsonConvert.SerializeObject(WrapInResult(new BittrexPrice())), "InvalidStatusCodeResponse", 203);

            // act
            var result = client.GetTicker("TestMarket");

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.ErrorMessage.Contains("InvalidStatusCodeResponse"));
        }

        private BittrexApiResult<T> WrapInResult<T>(T data, bool success = true, string message = null)
        {
            var result = new BittrexApiResult<T>();
            result.GetType().GetProperty("Result").SetValue(result, data);
            result.GetType().GetProperty("Success").SetValue(result, success);
            result.GetType().GetProperty("Message", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(result, message);
            return result;
        }

        private BittrexClient PrepareClient(string responseData, bool credentials = true)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IResponse>();
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<IRequest>();
            request.Setup(c => c.Headers).Returns(new WebHeaderCollection());
            request.Setup(c => c.GetResponse()).Returns(response.Object);

            var factory = new Mock<IRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);

            BittrexClient client = credentials ? new BittrexClient("Test", "Test") : new BittrexClient();
            client.RequestFactory = factory.Object;
            return client;
        }

        private BittrexClient PrepareExceptionClient(string responseData, string exceptionMessage, int statusCode, bool credentials = true)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);
            
            var we = new WebException();
            var r = new HttpWebResponse();
            var re = new HttpResponseMessage();

            typeof(HttpResponseMessage).GetField("_statusCode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(re, (HttpStatusCode)statusCode);
            typeof(HttpWebResponse).GetField("_httpResponseMessage", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(r, re);
            typeof(WebException).GetField("_message", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(we, exceptionMessage);
            typeof(WebException).GetField("_response", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(we, r);

            var response = new Mock<IResponse>();
            response.Setup(c => c.GetResponseStream()).Throws(we);

            var request = new Mock<IRequest>();
            request.Setup(c => c.Headers).Returns(new WebHeaderCollection());
            request.Setup(c => c.GetResponse()).Returns(response.Object);

            var factory = new Mock<IRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);

            BittrexClient client = credentials ? new BittrexClient("Test", "Test") : new BittrexClient();
            client.RequestFactory = factory.Object;
            return client;
        }
    }
}
