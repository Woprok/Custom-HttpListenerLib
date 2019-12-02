using System;
using System.IO;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Minimal interface required for correct serialization and deserialization of stream.
    /// </summary>
    public interface ISerializer//<T>
    {
        ///// <summary>
        ///// Serialization of stream to byte[]. 
        ///// </summary>
        //byte[] SerializeSendingData(T item, Stream memory);
        //
        ///// <summary>
        ///// Deserialization of stream to wanted type of T. 
        ///// </summary>
        //T DeserializeReceivedData(Stream stream);

        /// <summary>
        /// Generic fallback serialize method.
        /// </summary>
        ArraySegment<byte> SerializeSendingData<TE>(TE stream);
        
        /// <summary>
        /// Generic fallback deserialize method.
        /// </summary>
        TE DeserializeReceivedData<TE>(ArraySegment<byte> stream);
    }
}