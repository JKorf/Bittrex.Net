using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bittrex.Net.Objects;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Bittrex.Net.Converters.V3;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Bittrex.Net
{
    /// <summary>
    /// Client for the Bittrex socket API
    /// </summary>
    public class BittrexSocketClientV3: SocketClient
    {
        #region fields
        private static BittrexSocketClientV3Options defaultOptions = new BittrexSocketClientV3Options();
        private static BittrexSocketClientV3Options DefaultOptions => defaultOptions.Copy<BittrexSocketClientV3Options>();

        private const string HubName = "c3";
        
        #endregion

        #region ctor
        /// <summary>
        /// Creates a new socket client using the default options
        /// </summary>
        public BittrexSocketClientV3(): this(DefaultOptions)
        {
        }

        /// <summary>
        /// Creates a new socket client using the provided options
        /// </summary>
        /// <param name="options">Options to use for this client</param>
        public BittrexSocketClientV3(BittrexSocketClientV3Options options): base(options, options.ApiCredentials == null ? null : new BittrexAuthenticationProvider(options.ApiCredentials))
        {
            SocketFactory = new ConnectionFactory();

            SocketCombineTarget = 10;
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Set the default options for new clients
        /// </summary>
        /// <param name="options">Options to use for new clients</param>
        public static void SetDefaultOptions(BittrexSocketClientV3Options options)
        {
            defaultOptions = options;
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeKlinesAsync(string symbol, KlineInterval interval, Action<BittrexKlineUpdate> onUpdate)
        {
            return await Subscribe($"candle_{symbol}_{JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))}", onUpdate);
        }

        #endregion
        #region private

        private async Task<CallResult<UpdateSubscription>> Subscribe<T>(string channel, Action<T> handler)
        {
            return await Subscribe<JToken>(new ConnectionRequestV3("subscribe", channel), null, false, data =>
            {
                if (!data["A"].Any())
                    return;
                DecodeSignalRData(data, handler);
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override SocketConnection GetWebsocket(string address, bool authenticated)
        {
            // Override because signalr puts `/signalr/` add the end of the url
            var socketResult = sockets.Where(s => s.Value.Socket.Url == address + "/signalr/" && (s.Value.Authenticated == authenticated || !authenticated) && s.Value.Connected).OrderBy(s => s.Value.HandlerCount).FirstOrDefault();
            var result = socketResult.Equals(default(KeyValuePair<int, SocketConnection>)) ? null : socketResult.Value;
            if (result != null)
            {
                if (result.HandlerCount < SocketCombineTarget || (sockets.Count >= MaxSocketConnections && sockets.All(s => s.Value.HandlerCount >= SocketCombineTarget)))
                {
                    // Use existing socket if it has less than target connections OR it has the least connections and we can't make new
                    return result;
                }
            }

            // Create new socket
            var socket = CreateSocket(address);
            var socketWrapper = new SocketConnection(this, socket);
            foreach (var kvp in genericHandlers)
                socketWrapper.AddHandler(SocketSubscription.CreateForIdentifier(kvp.Key, false, kvp.Value));
            return socketWrapper;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> SubscribeAndWait(SocketConnection socket, object request, SocketSubscription subscription)
        {
            var btRequest = (ConnectionRequestV3) request;
            if (btRequest.RequestName != null)
            {
                var subResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<ConnectionResponse[]>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
                var data = subResult.Data?.First();
                if (!subResult.Success || data?.Success == false)
                {
                    _ = socket.Close(subscription);
                    return new CallResult<bool>(false, subResult.Error ?? new ServerError(data?.ErrorCode));
                }
            }

            subscription.Confirmed = true;
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<T>> QueryAndWait<T>(SocketConnection socket, object request)
        {
            var btRequest = (ConnectionRequestV3) request;
            var queryResult = await ((ISignalRSocket)socket.Socket).InvokeProxy<string>(btRequest.RequestName, btRequest.Parameters).ConfigureAwait(false);
            if (!queryResult.Success)
            {
                return new CallResult<T>(default, queryResult.Error);
            }

            var decResult = DecodeData(queryResult.Data);
            if (decResult == null)
            {
                return new CallResult<T>(default, new DeserializeError("Failed to decode data"));
            }

            var desResult = Deserialize<T>(decResult);
            if (!desResult.Success)
            {
                return new CallResult<T>(default, desResult.Error);
            }

            return new CallResult<T>(desResult.Data, null);
        }

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string) message["M"];
            var data = DecodeData((string) message["A"].FirstOrDefault());
            if (data == null)
                return method == "heartbeat";
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            var msg = message["A"];
            if (msg == null)
                return false;

            var method = (string)message["M"];

            return false;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            log.Write(LogVerbosity.Debug, "Starting authentication");
            var socket = (ISignalRSocket)s.Socket;
            var result = await socket.InvokeProxy<string>("GetAuthContext", authProvider.Credentials.Key!.GetString()).ConfigureAwait(false);
            if (!result.Success)
            {
                log.Write(LogVerbosity.Error, "Api key is probably invalid");
                return new CallResult<bool>(false, result.Error);
            }

            log.Write(LogVerbosity.Debug, "Auth context retrieved");
            var signed = authProvider.Sign(result.Data);
            var authResult = await socket.InvokeProxy<bool>("Authenticate", authProvider.Credentials.Key!.GetString(), signed).ConfigureAwait(false);
            if (!authResult.Success || !authResult.Data)
            {
                log.Write(LogVerbosity.Error, "Authentication failed, api secret is probably invalid");
                return new CallResult<bool>(false, authResult.Error ?? new ServerError("Api secret is probably invalid"));
            }

            log.Write(LogVerbosity.Info, "Authentication successful");
            return new CallResult<bool>(true, null);
        }

        /// <inheritdoc />
        protected override async Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            var bRequest = (ConnectionRequestV3)s.Request;
            var unsub = new ConnectionRequestV3("unsubscribe", ((string[])bRequest.Parameters[0])[0]);
            var result = false;
            var queryResult = await ((ISignalRSocket)connection.Socket).InvokeProxy<ConnectionResponse[]>(unsub.RequestName, unsub.Parameters).ConfigureAwait(false);
            
            return queryResult.Success;
        }

        /// <inheritdoc />
        protected override IWebsocket CreateSocket(string address)
        {
            var socket = (ISignalRSocket)base.CreateSocket(address);
            socket.SetHub(HubName);
            return socket;
        }

        private void DecodeSignalRData<T>(JToken data, Action<T> handler)
        {
            var actualData = (string)data["A"][0];
            var result = DecodeData(actualData);
            if (result == null)
                return;

            log.Write(LogVerbosity.Debug, "Socket received data: " + result);

            var decodeResult = Deserialize<T>(result);
            if (!decodeResult.Success)
                log.Write(LogVerbosity.Debug, "Failed to decode data: " + decodeResult.Error);

            handler(decodeResult.Data);
        }

        private string? DecodeData(string rawData)
        {
            try
            {
                var gzipData = Convert.FromBase64String(rawData);
                using var decompressedStream = new MemoryStream();
                using var compressedStream = new MemoryStream(gzipData);
                using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
                deflateStream.CopyTo(decompressedStream);
                decompressedStream.Position = 0;

                using var streamReader = new StreamReader(decompressedStream);
                var data = streamReader.ReadToEnd();
                if (data == "null")
                    return null;

                return data;
            }
            catch (Exception e)
            {
                log.Write(LogVerbosity.Info, "Exception in decode data: " + e.Message);
                return null;
            }
        }
        #endregion
        #endregion
    }
}
