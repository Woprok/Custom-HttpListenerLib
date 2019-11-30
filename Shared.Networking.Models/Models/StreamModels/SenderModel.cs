using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions.Exceptions;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc cref="ISender{T}"/>
    public sealed class SenderModel<T> : DataStreamModel, ISender<T>
    {
        /// <inheritdoc/>
        public event DataSent<T> OnDataSent;

        public SenderModel(TcpClient client, ISerializer<T> serializer) : base(client)
        {
            Serializer = serializer;
        }
        
        private async Task Send(byte[] data, CancellationToken token)
        {
            await ClientStream.WriteAsync(data, 0, data.Length, token);
            await ClientStream.FlushAsync(token);
        }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task SendAsync(T obj, CancellationToken token)
        {
            byte[] serializedData;
            await using (MemoryStream memory = new MemoryStream())
            {
                serializedData = Serializer.SerializeSendingData(obj, memory); 
            }
            if (IsConnected && !token.IsCancellationRequested)
            {
                await Task.Factory.StartNew(() => Send(serializedData, token), token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
                if (OnDataSent == null)
                    throw new EventNotSubscribedException("Sent method not subscribed!");
                await Task.Run(() => OnDataSent?.Invoke(this, obj), token);
            }
        }

        /// <inheritdoc/>
        public ISerializer<T> Serializer { get; }
    }
}