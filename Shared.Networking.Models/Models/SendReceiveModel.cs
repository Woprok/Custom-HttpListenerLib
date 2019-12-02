using System.Net.Sockets;
using System.Threading.Tasks;
using Shared.Common.Models;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models.StreamModels;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="ISendReceiveModel{T}"/>
    /// <inheritdoc cref="StartStopModel"/>
    public sealed class SendReceiveModel<T> : StartStopModel, ISendReceiveModel<T>
    {
        /// <inheritdoc/>
        public event SendReceiveModelDisconnected<T> OnSendReceiveModelDisconnected;

        /// <inheritdoc/>
        public event SendReceiveModelDataSent<T> OnSendReceiveModelDataSent;

        /// <inheritdoc/>
        public event SendReceiveModelDataReceived<T> OnSendReceiveModelDataReceived;

        public SendReceiveModel(long id, IClient client, ISerializer serializer)
        {
            Id = id;
            Client = client;
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public long Id { get; }

        /// <inheritdoc/>
        public IClient Client { get; }
        
        /// <inheritdoc/>
        public IReceiver Receiver { get; private set; }

        /// <inheritdoc/>
        public ISender Sender { get; private set; }

        /// <inheritdoc/>
        public bool IsValidated { get; set; } = false;

        /// <inheritdoc/>
        public bool IsValidConnection => IsValidated && Client.Connected;

        /// <inheritdoc/>
        public ISerializer Serializer { get; }

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            Receiver = new ReceiverModel(Client, Serializer);
            Sender = new SenderModel(Client, Serializer);
        }
        
        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Receiver.OnClientDisconnected += Receiver_OnClientDisconnected;
            Receiver.OnDataReceived += Receiver_OnDataReceived;
            Sender.OnDataSent += Sender_OnDataSent;
            Task.Factory.StartNew(() => Receiver.ReceiveAsync(CurrentCancellationToken), CurrentCancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void Sender_OnDataSent(ISender sender, T data) => OnSendReceiveModelDataSent?.Invoke(this, data);

        private void Receiver_OnDataReceived(IReceiver receiver, T data) => OnSendReceiveModelDataReceived?.Invoke(this, data);

        private void Receiver_OnClientDisconnected(IReceiver receiver) => OnSendReceiveModelDisconnected?.Invoke(this);

        /// <inheritdoc/>
        protected override void OnModelStop()
        {
            Receiver.OnClientDisconnected -= Receiver_OnClientDisconnected;
            Receiver.OnDataReceived -= Receiver_OnDataReceived;
            Sender.OnDataSent -= Sender_OnDataSent;
            Client.Close();
        }
        
        /// <inheritdoc/>
        public void Send(T obj) => Task.Run(() => Sender.SendAsync(obj, CurrentCancellationToken));
    }
}