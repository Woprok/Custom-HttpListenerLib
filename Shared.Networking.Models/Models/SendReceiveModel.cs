using System.Threading.Tasks;
using Shared.Common.Models;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.ListenerModels;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models.StreamModels;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="ISendReceiveModel"/>
    /// <inheritdoc cref="StartStopModel"/>
    public sealed class SendReceiveModel : StartStopModel, ISendReceiveModel
    {
        /// <inheritdoc/>
        public event SendReceiveModelDisconnected OnSendReceiveModelDisconnected;
        /// <inheritdoc/>
        public event SendReceiveModelError OnSendReceiveModelError;
        /// <inheritdoc/>
        public event SendReceiveModelDataSent<object> OnSendReceiveModelDataSent;
        /// <inheritdoc/>
        public event SendReceiveModelDataReceived<object> OnSendReceiveModelDataReceived;

        public SendReceiveModel(long id, ISocketClient socketClient, ISerializer serializer)
        {
            Id = id;
            SocketClient = socketClient;
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public long Id { get; }

        /// <inheritdoc/>
        public ISocketClient SocketClient { get; }
        
        /// <inheritdoc/>
        public IReceiver Receiver { get; private set; }

        /// <inheritdoc/>
        public ISender Sender { get; private set; }

        /// <inheritdoc/>
        public bool IsValidated { get; set; } = true;

        /// <inheritdoc/>
        public bool IsValidConnection => IsValidated && SocketClient.Connected;

        /// <inheritdoc/>
        public ISerializer Serializer { get; }

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            Receiver = new ReceiverModel(SocketClient, Serializer);
            Sender = new SenderModel(SocketClient, Serializer);
        }
        
        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Receiver.OnClientDisconnected += ReceiverOnClientDisconnected;
            Receiver.OnDataReceivedSuccess += ReceiverOnDataReceivedSuccess;
            Receiver.OnDataReceivedError += ReceiverOnDataReceivedError;
            Sender.OnDataSentSuccess += SenderOnDataSent;
            Sender.OnDataSentError += SenderOnDataSentError;
            Serializer.OnSerializerError += SerializerOnSerializerError;
            Task.Factory.StartNew(() => Receiver.ReceiveAsync(CurrentCancellationToken), CurrentCancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void SerializerOnSerializerError(ISerializer sender, System.Exception e, object context) => OnSendReceiveModelError?.Invoke(this);

        private void SenderOnDataSentError(ISender sender, System.Exception e, object data) => OnSendReceiveModelError?.Invoke(this);

        private void ReceiverOnDataReceivedError(IReceiver receiver, System.Exception e) => OnSendReceiveModelError?.Invoke(this);

        private void SenderOnDataSent(ISender sender, object data) => OnSendReceiveModelDataSent?.Invoke(this, data);

        private void ReceiverOnDataReceivedSuccess(IReceiver receiver, object data) => OnSendReceiveModelDataReceived?.Invoke(this, data);

        private void ReceiverOnClientDisconnected(IReceiver receiver) => OnSendReceiveModelDisconnected?.Invoke(this);

        /// <inheritdoc/>
        protected override void OnModelStop()
        {
            Receiver.OnClientDisconnected -= ReceiverOnClientDisconnected;
            Receiver.OnDataReceivedSuccess -= ReceiverOnDataReceivedSuccess;
            Receiver.OnDataReceivedError -= ReceiverOnDataReceivedError;
            Sender.OnDataSentSuccess -= SenderOnDataSent;
            Sender.OnDataSentError -= SenderOnDataSentError;
            Serializer.OnSerializerError -= SerializerOnSerializerError;
            SocketClient.Close();
        }
        
        /// <inheritdoc/>
        public void Send(object item) => Task.Run(() => Sender.SendAsync(item, CurrentCancellationToken));
    }
}