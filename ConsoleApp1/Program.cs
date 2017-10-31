using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bittrex.Net;
using Bittrex.Net.Objects;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BittrexClient("47e24077d51747f8a8f88fa4e6f08017", "71a97f070d8f4acbb67a97d6825a8f9b");
            var d = client.GetDepositHistory();
            //var dd = client.GetWithdrawalHistory();
        }
    }
}
