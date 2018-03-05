using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bittrex.Net.UnitTests.Core
{
    public class UtilityTests
    {
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("test", null)]
        [TestCase("test", "")]
        [TestCase(null, "test")]
        [TestCase("", "test")]
        public void SettingEmptyValuesForAPICredentials_Should_ThrowException(string key, string secret)
        {
            // arrange
            var client = PrepareClient("");

            // act
            // assert
            Assert.Throws(typeof(ArgumentException), () => client.SetApiCredentials(key, secret));
        }

        [TestCase()]
        public void SettingLogOutput_Should_RedirectLogOutput()
        {
            // arrange
            var stringBuilder = new StringBuilder();
            var client = PrepareClient(JsonConvert.SerializeObject(new BittrexPrice()), true, LogVerbosity.Debug, new StringWriter(stringBuilder));

            // act
            client.GetTicker("TestMarket");

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(stringBuilder.ToString()));
        }

        [TestCase()]
        public void SettingDefaults_Should_ImpactNewClients()
        {
            // arrange
            var stringBuilder = new StringBuilder();
            BittrexClient.SetDefaultOptions(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials("Test","Test2"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriter = new StringWriter(stringBuilder)
            });

            var client = PrepareClient(JsonConvert.SerializeObject(new BittrexPrice()), false);

            // act
            Assert.DoesNotThrow(() => client.GetBalances());

            // assert
            Assert.IsFalse(string.IsNullOrEmpty(stringBuilder.ToString()));
        }

        private BittrexClient PrepareClient(string responseData, bool withOptions = true, LogVerbosity verbosity = LogVerbosity.Warning, TextWriter tw = null)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IResponse>();
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<IRequest>();
            request.Setup(c => c.Headers).Returns(new WebHeaderCollection());
            request.Setup(c => c.Uri).Returns(new Uri("http://www.test.com"));
            request.Setup(c => c.GetResponse()).Returns(Task.FromResult(response.Object));

            var factory = new Mock<IRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);
            BittrexClient client;
            if (withOptions)
            {
                client = new BittrexClient(new BittrexClientOptions()
                {
                    ApiCredentials = new ApiCredentials("Test", "Test2"),
                    LogVerbosity = verbosity,
                    LogWriter = tw
                });
            }
            else
            {
                client = new BittrexClient();
            }
            client.RequestFactory = factory.Object;
            return client;
        }
    }
}
