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
        public event DataSentSuccess OnDataSentSuccess;
        /// <inheritdoc/>
        public event DataSentError OnDataSentError;

        public SenderModel(IClient client, ISerializer serializer) : base(client, serializer) { }
        
        private async Task Send(ArraySegment<byte> data, CancellationToken token)
        {
            await Client.SendAsync(data, WebSocketMessageType.Text, token);
        }

        /// <inheritdoc/>
        /// <exception cref="EventNotSubscribedException"/>
        public async Task SendAsync<TE>(TE item, CancellationToken token)
        {
            try
            {
                ArraySegment<byte> serializedData = Serializer.SerializeSendingData(item);
                if (Client.Connected && !token.IsCancellationRequested)
                {
                    await Task.Factory.StartNew(() => Send(serializedData, token), token, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
                    if (OnDataSentSuccess == null)
                        throw new EventNotSubscribedException("Sent success method not subscribed!");
                    await Task.Run(() => OnDataSentSuccess?.Invoke(this, item), token);
                }
            }
            catch (Exception e)
            {
                if (OnDataSentError == null)
                    throw new EventNotSubscribedException("Sent error method not subscribed!");
                await Task.Run(() => OnDataSentError?.Invoke(this, e, item), token);
            }
        }
    }
}