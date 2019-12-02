using System.Net;
using Shared.Common.Exceptions;
using Shared.Networking.Advanced.DataModels;
using Shared.Networking.Advanced.Entities.Messages;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models.StreamModels;

namespace Shared.Networking.Advanced
{
    public interface IServerNetworkModel
    {
        /// <inheritdoc cref="IPEndPoint"/>
        IPEndPoint IPEndPoint { get; }
        /// <inheritdoc cref="GeneralServerMessageModel"/>
        GeneralServerMessageModel ServerMessageModel { get; }
        /// <summary>
        /// Opens single message channel for message type communication.
        /// </summary>
        void OpenMessageChannel(IPEndPoint newEndPoint, ISerializer<CoreMessage> newSerializer);
    }

    /// <summary>
    /// Model responsible for everything server does over network and calling proper server members to handle incoming data, and send data...
    /// </summary>
    public class ServerNetworkModel : IServerNetworkModel
    {

        public ServerNetworkModel(IPEndPoint ipEndPoint)
        {
            IPEndPoint = ipEndPoint;
        }

        /// <inheritdoc/>
        public IPEndPoint IPEndPoint { get; }
        /// <inheritdoc/>
        public GeneralServerMessageModel ServerMessageModel { get; protected set; }
        /// <inheritdoc cref="ISerializer{T}"/>
        private ISerializer<CoreMessage> MessageSerializer { get; } = new BinaryFormatterSerializer<CoreMessage>();

        /// <summary>
        /// Both Parameters can be null, if parameter is null it will be replaced by default value from this class.
        /// </summary>
        /// <inheritdoc/>
        /// <exception cref="Shared.Common.Exceptions.InvalidCallException">Whenever attempt to initialize more than one Message models happen.</exception>
        public void OpenMessageChannel(IPEndPoint newEndPoint = null, ISerializer<CoreMessage> newSerializer = null)
        {
            if (ServerMessageModel != null)
                throw new InvalidCallException(nameof(OpenMessageChannel) + ": " + "Already initialized message channel.");
            if (newEndPoint == null)
                newEndPoint = IPEndPoint;
            if (newSerializer == null)
                newSerializer = MessageSerializer;

            ServerMessageModel = new GeneralServerMessageModel(newEndPoint, newSerializer);
        }
    }
}