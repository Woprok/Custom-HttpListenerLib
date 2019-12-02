using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc cref="ISender"/>
    public sealed class SenderModel : DataStreamModel, ISender
    {
        /// <inheritdoc/>
        public event DataSent<object> OnDataSent;

        public SenderModel(IClient client, ISerializer serializer) : base(client, serializer) { }
        
        private async Task Send(ArraySegment<byte> data, CancellationToken token)
        {
            await Client.SendAsync(data, WebSocketMessageType.Text, token);
        }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task SendAsync<TE>(TE item, CancellationToken token)
        {
            ArraySegment<byte> serializedData = Serializer.SerializeSendingData(item);

            if (IsConnected && !token.IsCancellationRequested)
            {
                await Task.Factory.StartNew(() => Send(serializedData, token), token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
                if (OnDataSent == null)
                    throw new EventNotSubscribedException("Sent method not subscribed!");
                await Task.Run(() => OnDataSent?.Invoke(this, item), token);
            }
        }
    }
}