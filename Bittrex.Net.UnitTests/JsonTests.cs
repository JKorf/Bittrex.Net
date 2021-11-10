using Bitfinex.Net.UnitTests;
using Bittrex.Net.Interfaces.Clients.Rest;
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
        public async Task ValidateSpotAccountCalls()
        {   
            await _comparer.ProcessSubject("Account", c => c.Account,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject("ExchangeData", c => c.ExchangeData,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject("Trading", c => c.Trading,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }
    }
}
