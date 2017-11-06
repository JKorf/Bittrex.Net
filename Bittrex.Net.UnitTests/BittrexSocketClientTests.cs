using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bittrex.Net.UnitTests
{
    public class BittrexSocketClientTests
    {
        [TestCase()]
        public void SubscribingToMarketDeltaStream_Should_TriggerWhenDeltaMessageIsReceived()
        {
            // arrange
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>()));
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket", (test) => result = test);

            var expected = new BittrexStreamDeltas()
            {
                Deltas = new List<BittrexMarketSummary>()
                {
                    new BittrexMarketSummary()
                    {
                        Ask = 1.1,
                        BaseVolume = 2.2,
                        Bid = 3.3,
                        Created = new DateTime(2017, 1, 1),
                        DisplayMarketName = null,
                        High = 4.4,
                        Last = 5.5,
                        Low = 6.6,
                        MarketName = "TestMarket",
                        OpenBuyOrders = 10,
                        OpenSellOrders = 20,
                        PrevDay = 7.7,
                        TimeStamp = new DateTime(2016, 1, 1),
                        Volume = 8.8
                    }
                }
            };

            // act
            var ex = JToken.FromObject(expected);
            var field = sub.GetType().GetField("Received", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var eventDelegate = (MulticastDelegate)field.GetValue(sub);

            foreach (var handler in eventDelegate.GetInvocationList())
                handler.Method.Invoke(handler.Target, new object[] { new List<JToken>() { ex } }); 

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(ObjectComparer.PublicInstancePropertiesEqual(result, expected.Deltas[0]));
        }

        [TestCase()]
        public void SubscribingToMarketDeltaStream_Should_NotTriggerWhenDeltaMessageForOtherMarketIsReceived()
        {
            // arrange
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>()));
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket1", (test) => result = test);

            var expected = new BittrexStreamDeltas()
            {
                Deltas = new List<BittrexMarketSummary>()
                {
                    new BittrexMarketSummary()
                    {
                        Ask = 1.1,
                        BaseVolume = 2.2,
                        Bid = 3.3,
                        Created = new DateTime(2017, 1, 1),
                        DisplayMarketName = null,
                        High = 4.4,
                        Last = 5.5,
                        Low = 6.6,
                        MarketName = "TestMarket2",
                        OpenBuyOrders = 10,
                        OpenSellOrders = 20,
                        PrevDay = 7.7,
                        TimeStamp = new DateTime(2016, 1, 1),
                        Volume = 8.8
                    }
                }
            };

            // act
            var ex = JToken.FromObject(expected);
            var field = sub.GetType().GetField("Received", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var eventDelegate = (MulticastDelegate)field.GetValue(sub);

            foreach (var handler in eventDelegate.GetInvocationList())
                handler.Method.Invoke(handler.Target, new object[] { new List<JToken>() { ex } });
            
            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsNull(result);
        }

        [TestCase()]
        public void WhenTheSocketReturnsEmptyDataTheEvent_Should_NotTrigger()
        {
            // arrange
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>()));
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket1", (test) => result = test);

            var expected = "";

            // act
            var ex = JToken.FromObject(expected);
            var field = sub.GetType().GetField("Received", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var eventDelegate = (MulticastDelegate)field.GetValue(sub);

            foreach (var handler in eventDelegate.GetInvocationList())
                handler.Method.Invoke(handler.Target, new object[] { new List<JToken>() { ex } });

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsNull(result);
        }

        [TestCase()]
        public void WhenUnsubscribingTheLastSubscriptionTheSocket_Should_Close()
        {
            // arrange
            bool stopCalled = false;
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket1", (test) => result = test);
            
            // act
            client.UnsubscribeFromStream(subscription.Result);
            Thread.Sleep(10);
            
            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(stopCalled);
        }

        [TestCase()]
        public void WhenUnsubscribingNotTheLastSubscriptionTheSocket_Should_NotClose()
        {
            // arrange
            bool stopCalled = false;
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket1", (test) => result = test);
            var subscription2 = client.SubscribeToMarketDeltaStream("TestMarket2", (test) => result = test);

            // act
            client.UnsubscribeFromStream(subscription.Result);

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsFalse(stopCalled);
        }

        [TestCase()]
        public void WhenUnsubscribingAllTheSocket_Should_Close()
        {
            // arrange
            bool stopCalled = false;
            var sub = new Subscription();

            var proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(sub);

            var socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);
            socket.Setup(s => s.Start());
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<string>())).Returns(socket.Object);

            BittrexMarketSummary result = null;
            var client = new BittrexSocketClient { ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexStreamRegistration>());
            var subscription = client.SubscribeToMarketDeltaStream("TestMarket1", (test) => result = test);
            var subscription2 = client.SubscribeToMarketDeltaStream("TestMarket2", (test) => result = test);

            // act
            client.UnsubscribeAllStreams();
            Thread.Sleep(10);

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(stopCalled);
        }
    }
}
