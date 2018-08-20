using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Requests;
using Moq;
using Newtonsoft.Json;
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetMarkets();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetCurrencies();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
        }

        [TestCase()]
        public void GetTicker_Should_RespondWithPrice()
        {
            // arrange
            var expected = new BittrexPrice() { Ask = 0.001m, Bid = 0.002m, Last = 0.003m };
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetTicker("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
        }

        [TestCase()]
        public void GetMarketSummary_Should_RespondWithMarketSummary()
        {
            // arrange
            var expected = new BittrexMarketSummary()
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
            };
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(new List<BittrexMarketSummary>() { expected })));

            // act
            var result = objects.Client.GetMarketSummary("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetMarketSummaries();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[2], expected[2]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data.Buy[0], expected.Buy[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data.Buy[1], expected.Buy[1]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data.Sell[0], expected.Sell[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data.Sell[1], expected.Sell[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetBuyOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetSellOrderBook("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetMarketHistory("TestMarket");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
        }

        [TestCase()]
        public void PlacingOrder_Should_ReturnOrderGuid()
        {
            // arrange
            var expected = new BittrexGuid() { Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301") };
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.PlaceOrder(OrderSide.Buy, "TestMarket", 1, 1);

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
        }

        [TestCase()]
        public void CancelOrder_Should_ReturnSuccess()
        {
            // arrange
            var expected = new object();
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.CancelOrder(new Guid());

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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetOpenOrders();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetBalance("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetBalances();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetDepositAddress("TestCurrency");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
        }

        [TestCase()]
        public void Withdraw_Should_ReturnWithdrawGuid()
        {
            // arrange
            var expected = new BittrexGuid()
            {
                Uuid = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
            };
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.Withdraw("TestCurrency", 1, "TestAddress");

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetOrder(new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"));

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data, expected));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetOrderHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetWithdrawalHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
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
            var objects = TestHelpers.PrepareClient(() => Construct(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test", "Test")
            }), JsonConvert.SerializeObject(WrapInResult(expected)));

            // act
            var result = objects.Client.GetDepositHistory();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[0], expected[0]));
            Assert.IsTrue(TestHelpers.PublicInstancePropertiesEqual(result.Data[1], expected[1]));
        }

        [TestCase()]
        public void FailedResponse_Should_GiveFailedResult()
        {
            // arrange
            var errorMessage = "TestErrorMessage";
            var objects = TestHelpers.PrepareClient(() => Construct(), JsonConvert.SerializeObject(WrapInResult<BittrexPrice>(null, false, errorMessage)));

            // act
            var result = objects.Client.GetTicker("TestMarket");

            // assert
            Assert.IsFalse(result.Success);
            Assert.AreNotEqual(0, result.Error.Code);
            Assert.IsTrue(result.Error.Message.Contains(errorMessage));
        }

        [Test]
        public void ReceivingServerError_Should_ReturnServerErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.PrepareExceptionClient<BittrexClient>("", "Unavailable", 504);

            // act
            var result = client.GetMarkets();

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.Message.Contains("Unavailable"));
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
            var sign = authProvider.AddAuthenticationToUriString("https://test.test-api.com", true);           

            // assert
            Assert.IsTrue(sign.StartsWith("https://test.test-api.com?apiKey=TestKey&nonce="));
        }

        [Test]
        public void AddingAuthenticationToRequest_Should_GiveCorrectRequestResult()
        {
            // arrange
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));
            var uri = new Uri("https://test.test-api.com");
            var request = new Request(WebRequest.CreateHttp(uri));

            // act
            var sign = authProvider.AddAuthenticationToRequest(request, true);

            // assert
            Assert.IsTrue(request.Headers["apisign"] == "3A82874271C0B4BE0F5DE44CB2CE7B39645AC93B07FD5570A700DC14C7524269B373DAFFA3A9BF1A2B6A318915D2ACEEC905163E574F34FF39EC62A676D2FBC2");
        }

        private BittrexClient Construct(BittrexClientOptions options = null)
        {
            if (options != null)
                return new BittrexClient(options);

            return new BittrexClient();
        }

        private BittrexApiResult<T> WrapInResult<T>(T data, bool success = true, string message = null) where T: class
        {
            var result = new BittrexApiResult<T>();
            result.GetType().GetProperty("Result").SetValue(result, data);
            result.GetType().GetProperty("Success").SetValue(result, success);
            result.GetType().GetProperty("Message", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(result, message);
            return result;
        }
    }
}
