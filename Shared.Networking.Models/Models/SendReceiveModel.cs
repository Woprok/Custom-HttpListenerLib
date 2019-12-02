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

        public SendReceiveModel(long id, IClient client, ISerializer<T> serializer)
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
        public IReceiver<T> Receiver { get; private set; }

        /// <inheritdoc/>
        public ISender<T> Sender { get; private set; }

        /// <inheritdoc/>
        public bool IsValidated { get; set; } = false;

        /// <inheritdoc/>
        public bool IsValidConnection => IsValidated && Client.Connected;

        /// <inheritdoc/>
        public ISerializer<T> Serializer { get; }

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            Receiver = new ReceiverModel<T>(Client, Serializer);
            Sender = new SenderModel<T>(Client, Serializer);
        }
        
        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Receiver.OnClientDisconnected += Receiver_OnClientDisconnected;
            Receiver.OnDataReceived += Receiver_OnDataReceived;
            Sender.OnDataSent += Sender_OnDataSent;
            Task.Factory.StartNew(() => Receiver.ReceiveAsync(CurrentCancellationToken), CurrentCancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void Sender_OnDataSent(ISender<T> sender, T data) => OnSendReceiveModelDataSent?.Invoke(this, data);

        private void Receiver_OnDataReceived(IReceiver<T> receiver, T data) => OnSendReceiveModelDataReceived?.Invoke(this, data);

        private void Receiver_OnClientDisconnected(IReceiver<T> receiver) => OnSendReceiveModelDisconnected?.Invoke(this);

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