namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Provides serializer used to encode and decode stream.
    /// </summary>
    public interface ISerializationModel<T>
    {
        /// <summary>
        /// Provides serialization methods used to encode and decode stream.
        /// </summary>
        ISerializer<T> Serializer { get; }
    }
}


