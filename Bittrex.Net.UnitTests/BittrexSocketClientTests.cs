using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
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
            //var client = PrepareClient();

            //// act
            //var subscription = client.SubscribeToMarketSummariesUpdate((test) => { });

            //// assert
            //proxy.Verify(p => p.Invoke<bool>("SubscribeToSummaryDeltas"), Times.Once);
        }

        [TestCase()]
        public void WhenSubscribingAuthenticatedStream_Should_InvokeAuthentication()
        {
            // arrange
            //var client = PrepareClient(true);
            //var authProvider = new BittrexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));
            //proxy.Setup(p => p.Invoke<string>("GetAuthContext", "TestKey")).Returns(Task.FromResult("TestAuthContext"));
            //proxy.Setup(p => p.Invoke<bool>("Authenticate", "TestKey", authProvider.Sign("TestAuthContext"))).Returns(Task.FromResult(true));

            //// act
            //var subscription = client.SubscribeToOrderUpdatesAsync((test) => { });

            //// assert
            //proxy.Verify(p => p.Invoke<string>("GetAuthContext", "TestKey"), Times.Once);
            //proxy.Verify(p => p.Invoke<bool>("Authenticate", "TestKey", authProvider.Sign("TestAuthContext")), Times.Once);
        }
    }
}
