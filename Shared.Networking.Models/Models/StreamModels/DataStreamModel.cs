using Shared.Networking.Models.Interfaces.ListenerModels;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc/>
    public abstract class DataStreamModel : IDataStreamModel
    {
        protected DataStreamModel(ISocketClient socketClient, ISerializer serializer)
        {
            SocketClient = socketClient;
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public ISocketClient SocketClient { get; }

        /// <inheritdoc/>
        public ISerializer Serializer { get; }
    }
}