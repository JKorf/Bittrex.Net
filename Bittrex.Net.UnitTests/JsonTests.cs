using Bitfinex.Net.UnitTests;
using Bittrex.Net.Objects;
using Bittrex.Net.UnitTests.TestImplementations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bittrex.Net.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        private JsonToObjectComparer<IBittrexClient> _comparer = new JsonToObjectComparer<IBittrexClient>((json) => TestHelpers.CreateResponseClient(json, new BittrexClientOptions()
        { ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "123"), OutputOriginalData = true }));

        [Test]
        public async Task ValidateSpotCalls()
        {   
            await _comparer.ProcessSubject(c => c,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }  
    }
}
