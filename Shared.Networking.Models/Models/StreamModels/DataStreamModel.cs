using System.IO;
using System.Net.Sockets;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <inheritdoc/>
    public abstract class DataStreamModel : IDataStreamModel
    {
        protected DataStreamModel(TcpClient client)
        {
            Client = client;
        }

        /// <inheritdoc/>
        public TcpClient Client { get; }

        /// <inheritdoc/>
        public Stream ClientStream => Client.GetStream();

        /// <inheritdoc/>
        public bool IsConnected => Client.Connected;
    }
}