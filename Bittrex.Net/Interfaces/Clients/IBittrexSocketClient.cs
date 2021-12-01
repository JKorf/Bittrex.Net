using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects.Models;
using Bittrex.Net.Objects.Models.Socket;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace Bittrex.Net.Interfaces.Clients.Socket
{
    /// <summary>
    /// Interface for the Bittrex V3 socket client
    /// </summary>
    public interface IBittrexSocketClient: ISocketClient
    {
        IBittrexSocketClientSpotMarket SpotStreams { get; }
    }
}