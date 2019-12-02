using System.IO;
using System.Net.Sockets;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc/>
    public abstract class DataStreamModel : IDataStreamModel
    {
        protected DataStreamModel(IClient client, ISerializer serializer)
        {
            Client = client;
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public IClient Client { get; }

        /// <inheritdoc/>
        public ISerializer Serializer { get; }

        /// <inheritdoc/>
        public bool IsConnected => Client.Connected;
    }
}