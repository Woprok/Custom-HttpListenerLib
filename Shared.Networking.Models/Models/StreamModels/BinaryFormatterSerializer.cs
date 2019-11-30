using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    /// <summary>
    /// Provides implementation of ISerializer by using BinaryFormatter.
    /// </summary>
    /// <inheritdoc/>
    public sealed class BinaryFormatterSerializer<T> : ISerializer<T>
    {
        private readonly BinaryFormatter serializer = new BinaryFormatter();
        
        /// <inheritdoc/>
        public byte[] SerializeSendingData(T obj, Stream memory)
        {
            serializer.Serialize(memory, obj);
            return ((MemoryStream)memory).ToArray();
        }

        /// <inheritdoc/>
        public T DeserializeReceivedData(Stream stream)
        {
            object deserialized = serializer.Deserialize(stream);
            return (T)deserialized;
        }
    }
}