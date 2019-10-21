using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Bittrex.Net.Objects;
using Bittrex.Net.UnitTests.TestImplementations;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using NUnit.Framework;

namespace Bittrex.Net.UnitTests
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
                    MinTradeSize = 0.0001m,
                    IsSponsored = null,
                    LogoUrl = null,
                    Notice = null
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
                    MinTradeSize = 1,
                    IsSponsored = true,
                    LogoUrl = "https://testurl",
                    Notice = "Test notice"
                }
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetMarkets();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
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
                    TransactionFee = 0.2m,
                    Notice = "Test notice"
                },
                new BittrexCurrency()
                {
                    BaseAddress = "TestAddress2",
                    IsActive = true,
                    CoinType = "TESTCOIN",
                    Currency = "TTN2",
                    CurrencyLong = "TestToken2",
                    MinConfirmation = 2,
                    TransactionFee = 3,
                    Notice = null
                },
            };

            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetCurrencies();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void GetTicker_Should_RespondWithPrice()
        {
            // arrange
            var expected = new BittrexPrice() { Ask = 0.001m, Bid = 0.002m, Last = 0.003m };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetTicker("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void GetMarketSummary_Should_RespondWithMarketSummary()
        {
            // arrange
            var expected = new List<BittrexMarketSummary>
            {
                new BittrexMarketSummary
                {
                    Ask = 0.001m,
                    Bid = 0.002m,
                    Last = 0.003m,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = 1.1m,
                    High = 2.2m,
                    Low = 3.3m,
                    OpenBuyOrders = 10,
                    OpenSellOrders = 20,
                    PrevDay = 4.4m,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = 5.5m
                }
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetMarketSummary("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected[0]));
        }

        [TestCase()]
        public void GetMarketSummaries_Should_RespondWithMarketSummariesList()
        {
            // arrange
            var expected = new List<BittrexMarketSummary>()
            {
                new BittrexMarketSummary()
                {
                    Ask = 0.001m,
                    Bid = 0.002m,
                    Last = 0.003m,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = 1.1m,
                    High = 2.2m,
                    Low = 3.3m,
                    OpenBuyOrders = 10,
                    OpenSellOrders = 20,
                    PrevDay = 4.4m,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = 5.5m
                },
                new BittrexMarketSummary()
                {
                    Ask = 0.006m,
                    Bid = 0.007m,
                    Last = 0.008m,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = 9.9m,
                    High = 10.10m,
                    Low = 11.11m,
                    OpenBuyOrders = 30,
                    OpenSellOrders = 40,
                    PrevDay = 12.12m,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = 13.13m
                },
                new BittrexMarketSummary()
                {
                    Ask = 0.006m,
                    Bid = 0.007m,
                    Last = null,
                    Created = new DateTime(2017, 1, 1),
                    MarketName = "TestMarket",
                    BaseVolume = null,
                    High = null,
                    Low = null,
                    OpenBuyOrders = null,
                    OpenSellOrders = null,
                    PrevDay = null,
                    TimeStamp = new DateTime(2016, 1, 1),
                    Volume = null
                }
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetMarketSummaries();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[2], expected[2]));
        }

        [TestCase()]
        public void GetOrderBook_Should_ReturnOrderBook()
        {
            // arrange
            var expected = new BittrexOrderBook()
            {
                Buy = new List<BittrexOrderBookEntry>()
                {
                    new BittrexOrderBookEntry(){Quantity = 1.1m, Rate = 2.2m},
                    new BittrexOrderBookEntry(){Quantity = 3.3m, Rate = 3.3m},
                },
                Sell = new List<BittrexOrderBookEntry>()
                {
                    new BittrexOrderBookEntry(){Quantity = 4.4m, Rate = 5.5m},
                    new BittrexOrderBookEntry(){Quantity = 6.6m, Rate = 7.7m},
                }
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetOrderBook("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.Buy.ToList()[0], expected.Buy.ToList()[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.Buy.ToList()[1], expected.Buy.ToList()[1]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.Sell.ToList()[0], expected.Sell.ToList()[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.Sell.ToList()[1], expected.Sell.ToList()[1]));
        }

        [TestCase()]
        public void GetBuyOrderBook_Should_ReturnBuyOrderBook()
        {
            // arrange

            var expected = new[]
            {
                new BittrexOrderBookEntry() {Quantity = 1.1m, Rate = 2.2m},
                new BittrexOrderBookEntry() {Quantity = 3.3m, Rate = 3.3m},
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetBuyOrderBook("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void GetSellOrderBook_Should_ReturnSellOrderBook()
        {
            // arrange

            var expected = new[]
            {
                new BittrexOrderBookEntry() {Quantity = 1.1m, Rate = 2.2m},
                new BittrexOrderBookEntry() {Quantity = 3.3m, Rate = 3.3m},
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetSellOrderBook("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void GetMarketHistory_Should_ReturnMarketHistoryList()
        {
            // arrange
            var expected = new List<BittrexMarketHistory>()
            {
                new BittrexMarketHistory()
                {
                    Quantity = 1.1m,
                    OrderType = OrderSide.Buy,
                    FillType = FillType.Fill,
                    Id = 1000000000000,
                    Price = 2.2m,
                    Timestamp = new DateTime(2017, 1, 1),
                    Total = 3.3m
                },
                new BittrexMarketHistory()
                {
                    Quantity = 4.4m,
                    OrderType = OrderSide.Sell,
                    FillType = FillType.PartialFill,
                    Id = 2000000000000,
                    Price = 5.5m,
                    Timestamp = new DateTime(2017, 1, 1),
                    Total = 6.6m
                }
            };
            var client = TestHelpers.CreateResponseClient(WrapInResult(expected));

            // act
            var result = client.GetMarketHistory("BTC-ETH");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void PlacingOrder_Should_ReturnOrderGuid()
        {
            // arrange
            var expected = new BittrexGuid() { Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301") };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.PlaceOrder(OrderSide.Buy, "BTC-ETH", 1, 1);

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void CancelOrder_Should_ReturnSuccess()
        {
            // arrange
            var expected = new object();
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.CancelOrder(new Guid());

            // assert
            Assert.IsTrue(result.Success);
        }

        [TestCase()]
        public void GetOpenOrders_Should_ReturnOpenOrdersList()
        {
            // arrange
            var expected = new List<BittrexOpenOrdersOrder>()
            {
                new BittrexOpenOrdersOrder
                {
                    Uuid = null,
                    Quantity = 1.1m,
                    OrderType = OrderSideExtended.LimitBuy,
                    Price = 2.2m,
                    Closed = null,
                    CancelInitiated = false,
                    CommissionPaid = 3.3m,
                    Condition = ConditionType.None,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = false,
                    IsConditional = false,
                    Limit = 4.4m,
                    Opened = new DateTime(2017, 1,1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 1.9m,
                    QuantityRemaining = 5.5m
                },
                new BittrexOpenOrdersOrder
                {
                    Uuid = null,
                    Quantity = 6.6m,
                    OrderType = OrderSideExtended.LimitSell,
                    Price = 7.7m,
                    Closed = new DateTime(2017, 1, 1),
                    CancelInitiated = true,
                    CommissionPaid = 8.8m,
                    Condition = ConditionType.None,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = true,
                    IsConditional = false,
                    Limit = 9.9m,
                    Opened = new DateTime(2017, 1,1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 11.11m,
                    QuantityRemaining = 10.1m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetOpenOrders();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void GetBalance_Should_ReturnBalance()
        {
            // arrange
            var expected = new BittrexBalance()
            {
                Currency = "TestCurrency",
                Available = 1.1m,
                Balance = 2.2m,
                CryptoAddress = "TestAddress",
                Pending = 3.3m,
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetBalance("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void GetBalances_Should_ReturnBalanceList()
        {
            // arrange
            var expected = new List<BittrexBalance>()
            {
                new BittrexBalance()
                {
                    Currency = "TestCurrency",
                    Available = 1.1m,
                    Balance = 2.2m,
                    CryptoAddress = "TestAddress",
                    Pending = 3.3m
                },
                new BittrexBalance()
                {
                    Currency = "TestCurrency",
                    Available = 4.4m,
                    Balance = 5.5m,
                    CryptoAddress = "TestAddress",
                    Pending = 6.6m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetBalances();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void GetDepositAddress_Should_ReturnDepositAddress()
        {
            // arrange
            var expected = new BittrexDepositAddress()
            {
                Currency = "TestCurrency",
                Address = "TestAddress"
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetDepositAddress("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void Withdraw_Should_ReturnWithdrawGuid()
        {
            // arrange
            var expected = new BittrexGuid()
            {
                Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.Withdraw("TestCurrency", 1, "TestAddress");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void GetOrder_Should_ReturnOrderInfo()
        {
            // arrange
            var expected = new BittrexAccountOrder()
            {
                Quantity = 1.1m,
                Price = 2.2m,
                CommissionPaid = 3.3m,
                OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                Condition = ConditionType.None,
                CancelInitiated = false,
                Exchange = "MarketExchange",
                PricePerUnit = null,
                QuantityRemaining = 4.4m,
                Closed = null,
                ImmediateOrCancel = false,
                ConditionTarget = null,
                IsConditional = false,
                Opened = new DateTime(2017, 1, 1),
                Limit = 5.5m,
                AccountId = "TestId",
                CommissionReserved = 6.6m,
                CommissionReserveRemaining = 7.7m,
                IsOpen = true,
                Reserved = 8.8m,
                ReserveRemaining = 9.9m,
                Sentinel = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                Type = OrderSideExtended.LimitBuy
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetOrder(new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"));

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data, expected));
        }

        [TestCase()]
        public void GetOrderHistory_Should_ReturnOrderHistoryList()
        {
            // arrange
            var expected = new List<BittrexOrderHistoryOrder>
            {
                new BittrexOrderHistoryOrder
                {
                    Quantity = 1.1m,
                    OrderType = OrderSideExtended.LimitBuy,
                    Price = 2.2m,
                    Closed = null,
                    Commission = 3.3m,
                    Condition = ConditionType.None,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = false,
                    IsConditional = false,
                    Limit = 4.4m,
                    TimeStamp = new DateTime(2017, 1, 1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 1.9m,
                    QuantityRemaining = 5.5m
                },
                new BittrexOrderHistoryOrder
                {
                    Quantity = 6.6m,
                    OrderType = OrderSideExtended.LimitSell,
                    Price = 7.7m,
                    Closed = new DateTime(2017, 1, 1),
                    Commission = 8.8m,
                    Condition = ConditionType.None,
                    ConditionTarget = null,
                    Exchange = "TestMarket",
                    ImmediateOrCancel = true,
                    IsConditional = false,
                    Limit = 9.9m,
                    TimeStamp = new DateTime(2017, 1, 1),
                    OrderUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PricePerUnit = 11.11m,
                    QuantityRemaining = 10.1m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetOrderHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
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
                    Amount = 1.1m,
                    Authorized = true,
                    Canceled = false,
                    InvalidAddress = false,
                    PaymentUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PendingPayment = false,
                    TransactionCost = 2.2m,
                    TransactionId = "TestId"
                },
                new BittrexWithdrawal()
                {
                    Opened = new DateTime(2016, 1, 1),
                    Currency = "TestCurrency",
                    Address = "TestAddress",
                    Amount = 3.3m,
                    Authorized = false,
                    Canceled = true,
                    InvalidAddress = true,
                    PaymentUuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    PendingPayment = true,
                    TransactionCost = 4.4m,
                    TransactionId = "TestId"
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetWithdrawalHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
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
                    Amount = 1.1m,
                    TransactionId = "TestId",
                    CryptoAddress = "TestAddress",
                    Id = 1000000000000,
                    Confirmations = 10,
                    LastUpdated = new DateTime(2017, 1, 1)
                },
                new BittrexDeposit()
                {
                    Currency = "TestCurrency",
                    Amount = 2.2m,
                    TransactionId = "TestId",
                    CryptoAddress = "TestAddress",
                    Id = 2000000000000,
                    Confirmations = 20,
                    LastUpdated = new DateTime(2016, 1, 1)
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(WrapInResult(expected));

            // act
            var result = client.GetDepositHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[0], expected[0]));
            Assert.IsTrue(TestHelpers.AreEqual(result.Data.ToList()[1], expected[1]));
        }

        [TestCase()]
        public void FailedResponse_Should_GiveFailedResult()
        {
            // arrange
            var errorMessage = "TestErrorMessage";
            var client = TestHelpers.CreateResponseClient(WrapInResult<object>(null, false, errorMessage));

            // act
            var result = client.GetTicker("BTC-ETH");

            // assert
            Assert.IsFalse(result.Success);
            Assert.AreNotEqual(0, result.Error.Code);
            Assert.IsTrue(result.Error.Message.Contains(errorMessage));
        }

        [TestCase()]
        public void HttpErrorResponse_Should_GiveFailedResult()
        {
            // arrange
            var errorMessage = "TestErrorMessage";
            var client = TestHelpers.CreateResponseClient(errorMessage, null, System.Net.HttpStatusCode.BadRequest);

            // act
            var result = client.GetTicker("BTC-ETH");

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Error.ToString().Contains(errorMessage));
        }

        [Test]
        public void ProvidingApiCredentials_Should_SaveApiCredentials()
        {
            // arrange
            // act
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // assert
            Assert.AreEqual(authProvider.Credentials.Key.GetString(), "TestKey");
            Assert.AreEqual(authProvider.Credentials.Secret.GetString(), "TestSecret");
        }

        [Test]
        public void SigningString_Should_GiveCorrectSignResult()
        {
            // arrange
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // act
            var sign = authProvider.Sign("TestStringToSign");

            // assert
            Assert.AreEqual(sign, "27D837EE7643CFA909E03F01F7794DC8D75127CDF02C826C3E57E9A11358972AD52001027A04133E8B15AD52EE73DDCE166229605D8709CFA769E2C7EE5CD988");
        }

        [Test]
        public void AddingAuthenticationToUriString_Should_GiveCorrectUriResult()
        {
            // arrange
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // act
            var sign = authProvider.AddAuthenticationToParameters("https://test.test-api.com", HttpMethod.Get, new Dictionary<string, object>(),  true);

            // assert
            Assert.IsTrue(sign.First().Key == "apiKey");
            Assert.IsTrue((string)sign.First().Value == "TestKey");
            Assert.IsTrue(sign.ElementAt(1).Key == "nonce");
        }

        [Test]
        public void AddingAuthenticationToRequest_Should_GiveCorrectRequestResult()
        {
            // arrange
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));
            var uri = new Uri("https://test.test-api.com");

            // act
            var sign = authProvider.AddAuthenticationToHeaders(uri.ToString(), HttpMethod.Get, new Dictionary<string, object>(), true);

            // assert
            Assert.IsTrue(sign.First().Value == "3A82874271C0B4BE0F5DE44CB2CE7B39645AC93B07FD5570A700DC14C7524269B373DAFFA3A9BF1A2B6A318915D2ACEEC905163E574F34FF39EC62A676D2FBC2");
        }

        private BittrexApiResult<T> WrapInResult<T>(T data, bool success = true, string message = null) where T: class
        {
            var result = new BittrexApiResult<T>();
            result.GetType().GetProperty("Result").SetValue(result, data);
            result.GetType().GetProperty("Success").SetValue(result, success);
            result.GetType().GetProperty("Message", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(result, message);
            return result;
        }

        [TestCase("BTC-USDT", true)]
        [TestCase("NANO-USDT", true)]
        [TestCase("NANO-BTC", true)]
        [TestCase("ETH-BTC", true)]
        [TestCase("BE-ETC", false)]
        [TestCase("NANO-USDTD", false)]
        [TestCase("BTCUSDT", false)]
        [TestCase("BTCUSD", false)]
        public void CheckValidBittrexSymbol(string symbol, bool isValid)
        {
            if (isValid)
                Assert.DoesNotThrow(() => symbol.ValidateBittrexSymbol());
            else
                Assert.Throws(typeof(ArgumentException), () => symbol.ValidateBittrexSymbol());
        }
    }
}
