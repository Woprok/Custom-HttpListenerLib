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
    }
}