using System;
using System.Text;
using Newtonsoft.Json;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer() { }

        /// <inheritdoc/>
        public event SerializerError OnSerializerError;

        /// <inheritdoc/>
        /// <exception cref="JsonSerializationException"/>
        public ArraySegment<byte> SerializeSendingData<TE>(TE item)
        {
            try
            {
                string serialized = JsonConvert.SerializeObject(item);
                byte[] buffer = Encoding.UTF8.GetBytes(serialized);
                return new ArraySegment<byte>(buffer);
            }
            catch (Exception e)
            {
                OnSerializerError?.Invoke(this, e, item);
                throw new JsonSerializationException("Serialization failed.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="JsonSerializationException"/>
        public TE DeserializeReceivedData<TE>(string stream)
        {
            try
            {
                return JsonConvert.DeserializeObject<TE>(stream);
            }
            catch (Exception e)
            {
                OnSerializerError?.Invoke(this, e, stream);
                throw new JsonSerializationException("Deserialization failed.");
            }
        }

        public object DeserializeReceivedData(string stream)
        {
            try
            {
                return JsonConvert.DeserializeObject(stream);
            }
            catch (Exception e)
            {
                OnSerializerError?.Invoke(this, e, stream);
                throw new JsonSerializationException("Deserialization failed.");
            }
        }
    }
}