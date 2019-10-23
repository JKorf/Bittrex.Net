using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Moq;
using Newtonsoft.Json;

namespace Bittrex.Net.UnitTests.TestImplementations
{
    public class TestHelpers
    {
        [ExcludeFromCodeCoverage]
        public static bool AreEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = self.GetType();
                var ignoreList = new List<string>(ignore);
                foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ignoreList.Contains(pi.Name))
                    {
                        continue;
                    }

                    var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                    var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                    if (pi.PropertyType.IsClass && !pi.PropertyType.Module.ScopeName.Equals("System.Private.CoreLib.dll"))
                    {
                        // Check of "CommonLanguageRuntimeLibrary" is needed because string is also a class
                        if (AreEqual(selfValue, toValue, ignore))
                        {
                            continue;
                        }

                        return false;
                    }

                    if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                    {
                        return false;
                    }
                }

                return true;
            }

            return self == to;
        }

        public static BittrexSocketClient CreateSocketClient(IWebsocket socket, BittrexSocketClientOptions options = null)
        {
            BittrexSocketClient client;
            client = options != null ? new BittrexSocketClient(options) : new BittrexSocketClient();
            client.SocketFactory = Mock.Of<IWebsocketFactory>();
            Mock.Get(client.SocketFactory).Setup(f => f.CreateWebsocket(It.IsAny<Log>(), It.IsAny<string>())).Returns(socket);
            return client;
        }

        public static IBittrexClient CreateClient(BittrexClientOptions options = null)
        {
            IBittrexClient client;
            client = options != null ? new BittrexClient(options) : new BittrexClient();
            client.RequestFactory = Mock.Of<IRequestFactory>();
            return client;
        }

        public static IBittrexClient CreateResponseClient(string response, BittrexClientOptions options = null)
        {
            var client = (BittrexClient)CreateClient(options);
            SetResponse(client, response);
            return client;
        }

        public static IBittrexClient CreateResponseClient<T>(T response, BittrexClientOptions options = null, HttpStatusCode code = HttpStatusCode.OK)
        {
            var client = (BittrexClient)CreateClient(options);
            SetResponse(client, JsonConvert.SerializeObject(response), code);
            return client;
        }

        public static IBittrexClient CreateAuthenticatedResponseClient<T>(T response, BittrexClientOptions options = null)
        {
            var client = (BittrexClient)CreateClient(options ?? new BittrexClientOptions(){ApiCredentials = new ApiCredentials("Test", "Test")});
            SetResponse(client, JsonConvert.SerializeObject(response));
            return client;
        }

        public static void SetResponse(RestClient client, string responseData, HttpStatusCode code = HttpStatusCode.OK)
        {
            var expectedBytes = Encoding.UTF8.GetBytes(responseData);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IResponse>();
            response.Setup(c => c.IsSuccessStatusCode).Returns(code == HttpStatusCode.OK);
            response.Setup(c => c.StatusCode).Returns(code);
            response.Setup(c => c.GetResponseStream()).Returns(Task.FromResult((Stream)responseStream));

            var request = new Mock<IRequest>();
            request.Setup(c => c.Uri).Returns(new Uri("http://www.test.com"));
            request.Setup(c => c.GetResponse(It.IsAny<CancellationToken>())).Returns(Task.FromResult(response.Object));

            var factory = Mock.Get(client.RequestFactory);
            factory.Setup(c => c.Create(It.IsAny<HttpMethod>(), It.IsAny<string>()))
                .Returns(request.Object);
        }
    }
}
