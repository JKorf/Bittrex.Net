using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex.Net.UnitTests
{
    public class BittrexSocketClientTests
    {
        Mock<Interfaces.IHubConnection> socket;
        Mock<IHubProxy> proxy;
                
        [TestCase()]
        public void Subscribing_Should_InvokeSubscribeDeltas()
        {
            // arrange
            var client = PrepareClient();

            // act
            var subscription = client.SubscribeToMarketSummariesUpdate((test) => { });

            // assert
            proxy.Verify(p => p.Invoke<bool>("SubscribeToSummaryDeltas"), Times.Once);
        }

        [TestCase()]
        public void WhenSubscribingAuthenticatedStream_Should_InvokeAuthentication()
        {
            // arrange
            var client = PrepareClient(true);
            var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));
            proxy.Setup(p => p.Invoke<string>("GetAuthContext", "TestKey")).Returns(Task.FromResult("TestAuthContext"));
            proxy.Setup(p => p.Invoke<bool>("Authenticate", "TestKey", authProvider.Sign("TestAuthContext"))).Returns(Task.FromResult(true));

            // act
            var subscription = client.SubscribeToOrderUpdatesAsync((test) => { });

            // assert
            proxy.Verify(p => p.Invoke<string>("GetAuthContext", "TestKey"), Times.Once);
            proxy.Verify(p => p.Invoke<bool>("Authenticate", "TestKey", authProvider.Sign("TestAuthContext")), Times.Once);
        }

        [TestCase()]
        public void WhenUnsubscribingTheLastSubscriptionTheSocket_Should_Close()
        {
            // arrange
            bool stopCalled = false;
            var client = PrepareClient();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);

            List<BittrexStreamMarketSummary> result = null;
            var subscription = client.SubscribeToMarketSummariesUpdate((test) => result = test);
            
            // act
            client.UnsubscribeFromStream(subscription.Data);
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
            var client = PrepareClient();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);
            
            var subscription = client.SubscribeToMarketSummariesUpdate(null);
            var subscription2 = client.SubscribeToMarketSummariesUpdate(null);

            // act
            client.UnsubscribeFromStream(subscription.Data);

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(subscription2.Success);
            Assert.IsFalse(stopCalled);
        }

        [TestCase()]
        public void WhenUnsubscribingAllTheSocket_Should_Close()
        {
            // arrange
            bool stopCalled = false;
            var client = PrepareClient();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);
            
            var subscription = client.SubscribeToMarketSummariesUpdate(null);
            var subscription2 = client.SubscribeToMarketSummariesUpdate(null);

            // act
            client.UnsubscribeAllStreams();
            Thread.Sleep(10);

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(subscription2.Success);
            Assert.IsTrue(stopCalled);
        }
        
        [TestCase()]
        public void Dispose_Should_ClearSubscriptions()
        {
            // arrange
            bool stopCalled = false;
            var client = PrepareClient();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>())).Callback(() => stopCalled = true);

            var subscription = client.SubscribeToMarketSummariesUpdate(null);
            var subscription2 = client.SubscribeToMarketSummariesUpdate(null);
            
            // act
            client.Dispose();
            Thread.Sleep(100);

            // assert 
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(subscription2.Success);
            Assert.IsTrue(stopCalled);
        }

        [TestCase()]
        public void WhenSocketDisconnectsWithSubscriptions_Should_Reconnect()
        {
            // arrange
            int startDone = 0;
            
            var client = PrepareClient();
            
            socket.Setup(s => s.Start()).Returns(Task.Run(() => Thread.Sleep(1))).Callback(() =>
            {
                startDone++;
                if (startDone == 1)
                {
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        socket.Setup(s => s.State).Returns(ConnectionState.Disconnected);
                        socket.Raise(s => s.Closed += null);
                    });
                }
            });

            var subscription = client.SubscribeToMarketSummariesUpdate(null);

            // act
            Thread.Sleep(3000);

            // assert
            Assert.IsTrue(subscription.Success);
            Assert.IsTrue(startDone == 2);
            proxy.Verify(p => p.Invoke<bool>("SubscribeToSummaryDeltas"), Times.Exactly(2));
        }

        private BittrexSocketClient PrepareClient(bool auth = false)
        {
            proxy = new Mock<IHubProxy>();
            proxy.Setup(r => r.Invoke<bool>(It.IsAny<string>())).Returns(Task.FromResult(true));
            proxy.Setup(r => r.Subscribe(It.IsAny<string>())).Returns(new Subscription());

            socket = new Mock<Interfaces.IHubConnection>();
            socket.Setup(s => s.Stop(It.IsAny<TimeSpan>()));
            socket.Setup(s => s.Start()).Returns(Task.Run(() => Thread.Sleep(1))).Callback(() => { socket.Raise(s => s.StateChanged += null, new StateChange(ConnectionState.Connecting, ConnectionState.Connected)); });
            socket.Setup(s => s.State).Returns(ConnectionState.Connected);
            socket.Setup(s => s.CreateHubProxy(It.IsAny<string>())).Returns(proxy.Object);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(s => s.Create(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket.Object);
                        
            var client = new BittrexSocketClient (auth? new BittrexSocketClientOptions() { ApiCredentials = new ApiCredentials("TestKey", "TestSecret")}: new BittrexSocketClientOptions()){ ConnectionFactory = factory.Object };
            client.GetType().GetField("connection", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, null);
            client.GetType().GetField("registrations", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, new List<BittrexRegistration>());
            client.GetType().GetField("reconnecting", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).SetValue(client, false);

            return client;
        }
    }
}
