using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net.Interfaces.Clients;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Objects.Models.Socket;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bittrex.Net.UnitTests
{
    internal class JsonSocketTests
    {
        [Test]
        public async Task ValidateKlineUpdateStreamJson()
        {
            await TestFileToObject<BittrexKlineUpdate>(@"JsonResponses/Socket/KlineUpdate.txt");
        }

        [Test]
        public async Task ValidateSummaryUpdateStreamJson()
        {
            await TestFileToObject<BittrexSymbolSummary>(@"JsonResponses/Socket/SummaryUpdate.txt");
        }

        [Test]
        public async Task ValidateTickerUpdateStreamJson()
        {
            await TestFileToObject<BittrexTick>(@"JsonResponses/Socket/TickerUpdate.txt");
        }

        [Test]
        public async Task ValidateTradeUpdateStreamJson()
        {
            await TestFileToObject<BittrexTradesUpdate>(@"JsonResponses/Socket/TradeUpdate.txt");
        }

        [Test]
        public async Task ValidateUserTradeUpdateStreamJson()
        {
            await TestFileToObject<BittrexExecutionUpdate>(@"JsonResponses/Socket/UserTradeUpdate.txt");
        }

        [Test]
        public async Task ValidateOrderUpdateStreamJson()
        {
            await TestFileToObject<BittrexOrderUpdate>(@"JsonResponses/Socket/OrderUpdate.txt");
        }

        private static async Task TestFileToObject<T>(string filePath, List<string> ignoreProperties = null)
        {
            var listener = new EnumValueTraceListener();
            Trace.Listeners.Add(listener);
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string json;
            try
            {
                var file = File.OpenRead(Path.Combine(path, filePath));
                using var reader = new StreamReader(file);
                json = await reader.ReadToEndAsync();
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            var result = JsonConvert.DeserializeObject<T>(json);
            JsonToObjectComparer<IBittrexSocketClient>.ProcessData("", result, json, ignoreProperties: new Dictionary<string, List<string>>
            {
                { "", ignoreProperties ?? new List<string>() }
            });
            Trace.Listeners.Remove(listener);
        }
    }

    internal class EnumValueTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }

        public override void WriteLine(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }
    }
}
