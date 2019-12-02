using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Models.StreamModels
{
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializer serializer = new JsonSerializer();
        
        /// <inheritdoc/>
        public ArraySegment<byte> SerializeSendingData<TE>(TE item)
        {
            string serialized = JsonConvert.SerializeObject(item);
            byte[] buffer = Encoding.UTF8.GetBytes(serialized);
            return new ArraySegment<byte>(buffer);
        }

        /// <inheritdoc/>
        public TE DeserializeReceivedData<TE>(string stream)
        {
            return JsonConvert.DeserializeObject<TE>(stream);
        }
    }
}