using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking
{
    /// <summary>
    /// Provides wrapper interface for client.
    /// </summary>
    public interface IClient
    {
        Stream GetStream();
        bool Connected { get; set; }
        int ReceiveBufferSize { get; set; }
        int SendBufferSize { get; set; }
        void Close();
        Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, CancellationToken cancellationToken);
        Task<string> ReceiveAsync();
    }

    public class SimpleClient : IClient
    {
        private readonly WebSocket socket;

        public Stream GetStream()
        {
            throw new System.NotImplementedException();
        }

        public bool Connected { get; set; }
        public int ReceiveBufferSize { get; set; }
        public int SendBufferSize { get; set; }
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, CancellationToken cancellationToken)
        {
            await socket.SendAsync(buffer, messageType, true, cancellationToken);
        }

        public async Task<string> ReceiveAsync()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[ReceiveBufferSize]);
            WebSocketReceiveResult result = null;
            
            using (MemoryStream dataMemoryStream = new MemoryStream())
            {
                do
                {
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    dataMemoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    return null;

                dataMemoryStream.Position = 0;
                using (StreamReader reader = new StreamReader(dataMemoryStream, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }

    /// <summary>
    /// Provides wrapper interface for client gathering.
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Enables new connections.
        /// </summary>
        void Start();
        /// <summary>
        /// Cancels all connections and block new connections.
        /// </summary>
        void Stop();
        /// <summary>
        /// Provides new client instance.
        /// </summary>
        IClient AcceptClientAsync();
    }

    /// <summary>
    /// Wrapper around httpListener.
    /// </summary>
    public class ExtendedHttpListener : IListener
    {
        private readonly HttpListener httpListener;
        public ExtendedHttpListener(IPEndPoint endPoint)
        {
            httpListener =  new HttpListener();
        }

        public void Start() => httpListener.Start();
        public void Stop() => httpListener.Stop();
        public IClient AcceptClientAsync() => null;
    }
}