using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;
using Shared.Common.Extensions;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.ListenerModels;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="DataModel"/>
    /// <inheritdoc cref="IGenericDataModel{TConnector}"/>
    public abstract class GenericDataModel<TConnector> : DataModel, IGenericDataModel<TConnector> where TConnector : class, IConnector, new()
    {
        private readonly IdHolder idHolder = new IdHolder();
        
        /// <summary>
        /// Private class that provides unique id for this models data exchangers.
        /// </summary>
        private class IdHolder
        {
            /// <summary>
            /// This value should not be accessed or used..
            /// </summary>
            private long currentId = 0;
            private readonly object idLock = new object();

            /// <summary>
            /// Provides new value that is unique (unless you somehow brute-forced few trillions of connections)
            /// </summary>
            public long NewId
            {
                get
                {
                    long newLastId;
                    Monitor.Enter(idLock);
                    try
                    {
                        ++currentId;
                        newLastId = currentId;
                    }
                    finally
                    {
                        Monitor.Exit(idLock);
                    }
                    return newLastId;
                }
            }
        }

        /// <summary>
        /// Default constructor calls StartRunning();
        /// Can be opted out by calling it will autorun = false
        /// </summary>>
        protected GenericDataModel(string ipEndPoint, ISerializer serializer, int defaultBufferSize = DefaultBufferSize, bool autorun = true) : base(ipEndPoint, defaultBufferSize)
        {
            Serializer = serializer;
            if (autorun)
            {
                StartRunning();
            }
        }

        /// <inheritdoc/>
        public TConnector Model { get; private set; }

        /// <inheritdoc/>
        public ISerializer Serializer { get; }

        /// <inheritdoc/>
        public Dictionary<long, ISendReceiveModel> DataExchangers { get; set; } = new Dictionary<long, ISendReceiveModel>();

        /// <summary>
        /// DataExchangers can be accessed from different methods at same time.
        /// </summary>
        protected readonly object SynchronizedDataExchangersAccess = new object();

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            Model = new TConnector { IpEndPoint = IpEndPoint };
            Model.Initialize();
        }

        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Model.OnNewClient += IncludeNewClientHandler;
            Model.Start();
        }

        /// <inheritdoc/>
        protected override void OnModelStop()
        {
            Model.Stop();
            Model.OnNewClient -= IncludeNewClientHandler;
            lock (SynchronizedDataExchangersAccess)
            {
                DataExchangers.ForEach(exchanger =>
                {
                    exchanger.Stop();
                    exchanger.OnSendReceiveModelDataReceived -= DataExchangerDataReceived;
                    exchanger.OnSendReceiveModelDataSent -= DataExchangerDataSent;
                    exchanger.OnSendReceiveModelDisconnected -= DataExchangerDisconnected;
                    exchanger.OnSendReceiveModelError -= DataExchangerError;
                });
                DataExchangers.Clear();
            }
        }

        private void IncludeNewClientHandler(ISocketClient newclient)
        {
            newclient.BufferSize = BufferSize;
            SendReceiveModel exchanger = new SendReceiveModel(idHolder.NewId, newclient, Serializer);
            exchanger.Initialize();
            exchanger.OnSendReceiveModelDataReceived += DataExchangerDataReceived;
            exchanger.OnSendReceiveModelDataSent += DataExchangerDataSent;
            exchanger.OnSendReceiveModelDisconnected += DataExchangerDisconnected;
            exchanger.OnSendReceiveModelError += DataExchangerError;
            exchanger.Start();

            lock (SynchronizedDataExchangersAccess)
            {
                DataExchangers.Add(exchanger.Id, exchanger);
            }
        }
        
        /// <inheritdoc/>
        public ImmutableDictionary<long, ISendReceiveModel> GetExchangers()
        {
            lock (SynchronizedDataExchangersAccess)
            {
                return DataExchangers.ToImmutableDictionary();
            }
        }

        /// <inheritdoc/>
        public ImmutableDictionary<long, ISendReceiveModel> GetValidatedExchangers()
        {
            lock (SynchronizedDataExchangersAccess)
            {
                return DataExchangers.Where(item => item.IsValidConnection).ToImmutableDictionary();
            }
        }

        /// <inheritdoc/>
        public ImmutableDictionary<long, ISendReceiveModel> GetValidatedExchangerByIds(HashSet<long> ids)
        {
            lock (SynchronizedDataExchangersAccess)
            {
                return DataExchangers.Where(item => item.IsValidConnection && ids.Contains(item.Id)).ToImmutableDictionary();
            }
        }

        /// <inheritdoc/>
        public void SendToAllValidConnections(object message)
        {
            lock (SynchronizedDataExchangersAccess)
            {
                DataExchangers.Where(item => item.IsValidConnection).ForEach(item => item.Send(message));
            }
        }

        /// <inheritdoc/>
        public void SendToAll(object message)
        {
            lock (SynchronizedDataExchangersAccess)
            {
                DataExchangers.ForEach(item => item.Send(message));
            }
        }

        /// <inheritdoc/>
        public virtual void DataExchangerDisconnected(ISendReceiveModel exchangerModel)
        {
            RemoveClientHandlers(exchangerModel);
        }

        private void RemoveClientHandlers(ISendReceiveModel exchangerModel)
        {
            exchangerModel.Stop();
            exchangerModel.OnSendReceiveModelDataReceived -= DataExchangerDataReceived;
            exchangerModel.OnSendReceiveModelDataSent -= DataExchangerDataSent;
            exchangerModel.OnSendReceiveModelDisconnected -= DataExchangerDisconnected;
            exchangerModel.OnSendReceiveModelError -= DataExchangerError;

            lock (SynchronizedDataExchangersAccess)
            {
                DataExchangers.Remove(exchangerModel.Id);
            }
        }

        /// <inheritdoc/>
        public virtual void DataExchangerDataSent(ISendReceiveModel sender, object data) { }

        /// <inheritdoc/>
        public virtual void DataExchangerDataReceived(ISendReceiveModel exchangerModel, object data) { }

        /// <inheritdoc/>
        public virtual void DataExchangerError(ISendReceiveModel receiver) { }

        /// <inheritdoc/>
        public void StartRunning()
        {
            Initialize();
            Start();
        }
    }
}