using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions.Exceptions;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc cref="IReceiver{T}"/>
    public sealed class ReceiverModel<T> : DataStreamModel, IReceiver<T>
    {
        /// <inheritdoc/>
        public event DataReceived<T> OnDataReceived;

        /// <inheritdoc/>
        public event ClientDisconnected<T> OnClientDisconnected;

        public ReceiverModel(TcpClient client, ISerializer<T> serializer) : base(client)
        {
            Serializer = serializer;
        }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task ReceiveAsync(CancellationToken token)
        {
            byte[] buffer = new byte[Client.ReceiveBufferSize];
            while (IsConnected && !token.IsCancellationRequested)
            {
                MemoryStream dataMemoryStream;
                T deserializedObject;
                await using (dataMemoryStream = new MemoryStream())
                {
                    int bytesRead = await ClientStream.ReadAsync(buffer, 0, buffer.Length, token);
                    while (bytesRead > buffer.Length)
                    {
                        dataMemoryStream.Write(buffer, 0, bytesRead);
                        bytesRead = await ClientStream.ReadAsync(buffer, 0, buffer.Length, token);
                    }
                    
                    dataMemoryStream.Write(buffer, 0, bytesRead);
                    dataMemoryStream.Position = 0;


                    if (bytesRead == 0)
                    {
                        if (OnClientDisconnected == null)
                            throw new EventNotSubscribedException("Disconnect method in Receiver not subscribed!");
                        await Task.Run(() => OnClientDisconnected?.Invoke(this), token);
                    }
                    else
                    {
                        deserializedObject = Serializer.DeserializeReceivedData(dataMemoryStream);
                        if (OnDataReceived == null)
                            throw new EventNotSubscribedException("Receive method not subscribed!");
                        await Task.Run(() => OnDataReceived?.Invoke(this, deserializedObject), token);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public ISerializer<T> Serializer { get; }
    }
}