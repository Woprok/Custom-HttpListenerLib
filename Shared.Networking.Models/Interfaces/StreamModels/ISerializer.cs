using System;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Event for announcing serializing & deserializing failure.
    /// </summary>
    public delegate void SerializerError(ISerializer sender, Exception e, object context);

    /// <summary>
    /// Minimal interface required for correct serialization and deserialization of stream.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Announces any error that occured during serialization.
        /// </summary>
        event SerializerError OnSerializerError;

        /// <summary>
        /// Generic fallback serialize method.
        /// </summary>
        ArraySegment<byte> SerializeSendingData<TE>(TE item);
        
        /// <summary>
        /// Generic fallback deserialize method.
        /// </summary>
        TE DeserializeReceivedData<TE>(string stream);
        /// <summary>
        /// Generic fallback deserialize method.
        /// </summary>
        object DeserializeReceivedData(string stream);
    }
}