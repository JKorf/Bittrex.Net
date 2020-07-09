using System;
using System.Collections.Generic;
using System.Text;

namespace Bittrex.Net.Sockets
{
    public class ConnectionResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
    }
}
