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

        //public byte[] SerializeSendingData(T obj, Stream memory)
        //{
        //    throw new NotImplementedException();
        //}
        //
        //public T DeserializeReceivedData(Stream stream)
        //{
        //    throw new NotImplementedException();
        //}

        public ArraySegment<byte> SerializeSendingData<TE>(TE item)
        {
            string serialized = JsonConvert.SerializeObject(item);
            byte[] buffer = Encoding.UTF8.GetBytes(serialized);
            return new ArraySegment<byte>(buffer);
        }
        public TE DeserializeReceivedData<TE>(ArraySegment<byte> stream)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>();


            return JsonConvert.DeserializeObject<TE>(stream);
        }
    }
}