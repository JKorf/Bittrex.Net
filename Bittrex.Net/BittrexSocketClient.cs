using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net.Errors;
using Bittrex.Net.Logging;
using Bittrex.Net.Objects;
using Newtonsoft.Json;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Implementations;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Bittrex.Net
{
    public class BittrexSocketClient: BittrexAbstractClient, IDisposable
    {
        #region fields
        private const string BaseAddress = "https://www.bittrex.com/";

        private const string HubName = "coreHub";
        private const string UpdateEvent = "updateSummaryState";

        private static Interfaces.IHubConnection connection;

        private readonly List<BittrexStreamRegistration> localRegistrations;
        private static readonly List<BittrexStreamRegistration> registrations = new List<BittrexStreamRegistration>();
        private static int lastStreamId;

        private static bool reconnecting;

        private static readonly object streamIdLock = new object();
        private static readonly object connectionLock = new object();
        private static readonly object registrationLock = new object();
        
        private int NextStreamId
        {
            get
            {
                lock (streamIdLock)
                {
                    lastStreamId += 1;
                    return lastStreamId;
                }
            }
        }
        #endregion

        #region properties
        public IConnectionFactory ConnectionFactory = new ConnectionFactory();
        #endregion

        public static event Action ConnectionLost;
        public static event Action ConnectionRestored;

        #region ctor
        public BittrexSocketClient()
        {
            localRegistrations = new List<BittrexStreamRegistration>();
        }

        ~BittrexSocketClient()
        {
            Dispose(false);
        }
        #endregion

        #region methods
        #region public
        /// <summary>
        /// Subscribes to updates on a specific market
        /// </summary>
        /// <param name="marketName">The name of the market to subscribe on</param>
        /// <param name="onUpdate">The update event handler</param>
        /// <returns></returns>
        public BittrexApiResult<int> SubscribeToMarketDeltaStream(string marketName, Action<BittrexMarketSummary> onUpdate)
        {
            log.Write(LogVerbosity.Debug, $"Going to subscribe to {marketName}");
            CreateConnection();

            ConnectionState state;
            lock (connection)
                state = connection.State;

            if (state == ConnectionState.Disconnected)
            {
                log.Write(LogVerbosity.Debug, "Starting connection to bittrex server");
                if (!WaitForConnection())
                {
                    return ThrowErrorMessage<int>(BittrexErrors.GetError(BittrexErrorKey.CantConnectToServer));
                }
            }

            var registration = new BittrexStreamRegistration() {Callback = onUpdate, MarketName = marketName, StreamId = NextStreamId};
            lock (registrationLock)
            {
                registrations.Add(registration);    
                localRegistrations.Add(registration);
            }
            return new BittrexApiResult<int>() {Result = registration.StreamId, Success = true};
        }

        /// <summary>
        /// Unsubsribe from updates of a specific stream using the stream id acquired when subscribing
        /// </summary>
        /// <param name="streamId">The stream id of the stream to unsubscribe</param>
        public void UnsubscribeFromStream(int streamId)
        {
            log.Write(LogVerbosity.Debug, $"Unsubscribing stream with id {streamId}");
            lock (registrationLock)
            {
                localRegistrations.RemoveAll(r => r.StreamId == streamId);
                registrations.RemoveAll(r => r.StreamId == streamId);
            }
            
            CheckStop();
        }

        /// <summary>
        /// Unsubscribes all streams on this client
        /// </summary>
        public void UnsubscribeAllStreams()
        {
            log.Write(LogVerbosity.Debug, "Unsubscribing all streams on this client");
            lock (registrationLock)
            {
                registrations.RemoveAll(r => localRegistrations.Contains(r));
                localRegistrations.Clear();
            }

            CheckStop();
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        #region private
        private void CheckStop()
        {
            bool shouldStop;
            lock (registrationLock)
                shouldStop = !registrations.Any();

            if (shouldStop)
            {
                Task.Run(() =>
                {
                    lock (connectionLock)
                    {
                        log.Write(LogVerbosity.Debug, "No more subscriptions, stopping the socket");
                        connection.Stop(TimeSpan.FromSeconds(1));
                    }
                });
            }
        }

        private bool WaitForConnection()
        {
            lock (connectionLock)
            {
                var waitEvent = new ManualResetEvent(false);
                var waitDelegate = new Action<StateChange>((state) =>
                {
                    if (state.NewState == ConnectionState.Connected ||
                        (state.NewState == ConnectionState.Disconnected &&
                         state.OldState == ConnectionState.Connecting))
                        waitEvent.Set();
                });

                connection.StateChanged += waitDelegate;
                connection.Start();

                waitEvent.WaitOne();
                connection.StateChanged -= waitDelegate;
                return connection.State == ConnectionState.Connected;
            }
        }

        private void CreateConnection()
        {
            lock (connectionLock)
            {
                if (connection != null)
                    return;

                connection = ConnectionFactory.Create(BaseAddress);
                var proxy = connection.CreateHubProxy(HubName);

                connection.StateChanged += state => log.Write(LogVerbosity.Debug, $"Socket state: {state.OldState} -> {state.NewState}");
                connection.Closed += () =>
                {
                    log.Write(LogVerbosity.Debug, "Socket closed");
                    if (!reconnecting && registrations.Any())
                    {
                        reconnecting = true;
                        ConnectionLost?.Invoke();
                        Task.Run(() => TryReconnect());
                    }
                };
                connection.Error += exception => log.Write(LogVerbosity.Error, $"Socket error: {exception.Message}");
                connection.ConnectionSlow += () => log.Write(LogVerbosity.Warning, "Socket connection slow");

                Subscription sub = proxy.Subscribe(UpdateEvent);
                sub.Received += jsonData =>
                {
                    if (jsonData.Count == 0)
                        return;

                    try
                    {
                        var data = JsonConvert.DeserializeObject<BittrexStreamDeltas>(jsonData[0].ToString());
                        foreach (var delta in data.Deltas)
                        foreach (var update in registrations.Where(r => r.MarketName == delta.MarketName))
                            update.Callback(delta);
                    }
                    catch (Exception e)
                    {
                        log.Write(LogVerbosity.Warning, $"Received an event but an unknown error occured. Message: {e.Message}, Received data: {jsonData[0]}");
                    }
                };
            }
        }

        private void TryReconnect()
        {
            if (registrations.Any())
            {
                if (!WaitForConnection())
                {
                    Thread.Sleep(5000);
                    TryReconnect();
                }
                else
                {
                    reconnecting = false;
                    ConnectionRestored?.Invoke();
                }
            }
            reconnecting = false;
        }

        private void Dispose(bool disposing)
        {
            base.Dispose();
            UnsubscribeAllStreams();
        }
        #endregion
        #endregion
    }
}
