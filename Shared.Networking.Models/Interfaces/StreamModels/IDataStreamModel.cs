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
        IClient Client { get; }

        /// <summary>
        /// Returns ISerializer currently in use by model.
        /// </summary>
        ISerializer Serializer { get; }
    }
}