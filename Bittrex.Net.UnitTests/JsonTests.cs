using Bittrex.Net.Objects;
using Bittrex.Net.UnitTests.TestImplementations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using Bittrex.Net.Interfaces.Clients;

namespace Bittrex.Net.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        private JsonToObjectComparer<IBittrexClient> _comparer = new JsonToObjectComparer<IBittrexClient>((json) => TestHelpers.CreateResponseClient(json, new BittrexClientOptions()
        { 
            ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "123"), 
            SpotApiOptions = new CryptoExchange.Net.Objects.RestApiClientOptions
            {
                OutputOriginalData = true,
                RateLimiters = new List<IRateLimiter>()
            }
        }));

        [Test]
        public async Task ValidateSpotAccountCalls()
        {   
            await _comparer.ProcessSubject("Account", c => c.SpotApi.Account,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject("ExchangeData", c => c.SpotApi.ExchangeData,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject("Trading", c => c.SpotApi.Trading,
                parametersToSetNull: new[] { "previousPageToken", "quoteQuantity" });
        }
    }
}
