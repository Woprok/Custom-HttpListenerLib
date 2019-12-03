using Shared.Networking.Models.Interfaces.ListenerModels;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Provides shared methods for Sending and Receiving over net.
    /// </summary>
    public interface IDataStreamModel
    {
        /// <summary>
        /// Returns IClient currently in use by model.
        /// </summary>
        ISocketClient SocketClient { get; }

        /// <summary>
        /// Returns ISerializer currently in use by model.
        /// </summary>
        ISerializer Serializer { get; }
    }
}