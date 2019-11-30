using System.IO;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Minimal interface required for correct serialization and deserialization of stream.
    /// </summary>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serialization of stream to byte[]. 
        /// </summary>
        byte[] SerializeSendingData(T obj, Stream memory);

        /// <summary>
        /// Deserialization of stream to wanted type of T. 
        /// </summary>
        T DeserializeReceivedData(Stream stream);
    }
}