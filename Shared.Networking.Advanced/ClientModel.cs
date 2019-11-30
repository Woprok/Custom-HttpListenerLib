using System.Net;
using Shared.Common.Exceptions.Exceptions;
using Shared.Networking.Advanced.DataModels;
using Shared.Networking.Advanced.Entities.Messages;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models.StreamModels;

namespace Shared.Networking.Advanced
{
    public interface IClientNetworkModel
    {
        /// <inheritdoc cref="IPEndPoint"/>
        IPEndPoint IPEndPoint { get; }
        /// <inheritdoc cref="GeneralClientMessageModel"/>
        GeneralClientMessageModel ClientMessageModel { get; }
        /// <summary>
        /// Opens single message channel for message type communication.
        /// </summary>
        void OpenMessageChannel(IPEndPoint newEndPoint, ISerializer<CoreMessage> newSerializer);
    }

    public class ClientNetworkModel : IClientNetworkModel
    {
        public ClientNetworkModel(IPEndPoint ipEndPoint)
        {
            IPEndPoint = ipEndPoint;
        }

        /// <inheritdoc/>
        public IPEndPoint IPEndPoint { get; }
        /// <inheritdoc/>
        public GeneralClientMessageModel ClientMessageModel { get; protected set; }
        /// <inheritdoc cref="ISerializer{T}"/>
        private ISerializer<CoreMessage> DefaultMessageSerializer { get; } = new BinaryFormatterSerializer<CoreMessage>();

        /// <inheritdoc/>
        /// <exception cref="InvalidCallException">Whenever attempt to initialize more than one Message models happen.</exception>
        public void OpenMessageChannel(IPEndPoint newEndPoint, ISerializer<CoreMessage> newSerializer)
        {
            if (ClientMessageModel != null)
                throw new InvalidCallException(nameof(OpenMessageChannel) + ": " + "Already initialized message channel.");
            if (newEndPoint == null)
                newEndPoint = IPEndPoint;
            if (newSerializer == null)
                newSerializer = DefaultMessageSerializer;

            ClientMessageModel = new GeneralClientMessageModel(newEndPoint, newSerializer);
        }
    }
}