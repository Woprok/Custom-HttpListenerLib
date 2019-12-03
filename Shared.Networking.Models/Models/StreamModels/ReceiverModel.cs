using System;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions;
using Shared.Networking.Models.Interfaces.ListenerModels;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc cref="IReceiver"/>
    public sealed class ReceiverModel : DataStreamModel, IReceiver
    {
        /// <inheritdoc/>
        public event DataReceivedSuccess OnDataReceivedSuccess;

        /// <inheritdoc/>
        public event DataReceivedError OnDataReceivedError;

        /// <inheritdoc/>
        public event ClientDisconnected OnClientDisconnected;

        public ReceiverModel(ISocketClient socketClient, ISerializer serializer) : base(socketClient, serializer) { }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task ReceiveAsync(CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[SocketClient.BufferSize];
                while (SocketClient.Connected && !token.IsCancellationRequested)
                {
                    object deserializedObject;
                    string content = await SocketClient.ReceiveAsync();

                    if (string.IsNullOrEmpty(content))
                    {
                        if (OnClientDisconnected == null)
                            throw new EventNotSubscribedException("Disconnect method in Receiver not subscribed!");
                        await Task.Run(() => OnClientDisconnected?.Invoke(this), token);
                    }
                    else
                    {
                        deserializedObject = Serializer.DeserializeReceivedData(content);
                        if (OnDataReceivedSuccess == null)
                            throw new EventNotSubscribedException("Receive method not subscribed!");
                        await Task.Run(() => OnDataReceivedSuccess?.Invoke(this, deserializedObject), token);
                    }
                }
            }
            catch (Exception e)
            {
                if (OnDataReceivedError == null)
                    throw new EventNotSubscribedException("Receive error method not subscribed!");
                await Task.Run(() => OnDataReceivedError?.Invoke(this, e), token);
            }
        }
    }
}