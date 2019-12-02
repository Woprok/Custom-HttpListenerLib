using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc cref="IReceiver"/>
    public sealed class ReceiverModel : DataStreamModel, IReceiver
    {
        /// <inheritdoc/>
        public event DataReceived<object> OnDataReceived;

        /// <inheritdoc/>
        public event ClientDisconnected OnClientDisconnected;

        public ReceiverModel(IClient client, ISerializer serializer) : base(client, serializer) { }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task ReceiveAsync(CancellationToken token)
        {
            byte[] buffer = new byte[Client.ReceiveBufferSize];
            while (IsConnected && !token.IsCancellationRequested)
            {
                object deserializedObject;
                string content = Client.ReceiveAsync();


                if (string.IsNullOrEmpty(content))
                {
                    if (OnClientDisconnected == null)
                        throw new EventNotSubscribedException("Disconnect method in Receiver not subscribed!");
                    await Task.Run(() => OnClientDisconnected?.Invoke(this), token);
                }
                else
                {
                    deserializedObject = Serializer.DeserializeReceivedData<dynamic>(content);
                    if (OnDataReceived == null)
                        throw new EventNotSubscribedException("Receive method not subscribed!");
                    await Task.Run(() => OnDataReceived?.Invoke(this, deserializedObject), token);
                }
            }
        }
    }
}