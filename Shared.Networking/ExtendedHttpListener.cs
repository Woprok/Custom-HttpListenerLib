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
        bool Connected { get; }
        int BufferSize { get; set; }
        void Close();
        Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, CancellationToken cancellationToken);
        Task<string> ReceiveAsync();
    }

    public class SocketClient : IClient
    {
        public SocketClient(WebSocket socket)
        {
            this.socket = socket;
        }

        private readonly WebSocket socket;

        public bool Connected => socket.State == WebSocketState.Open;
        public int BufferSize { get; set; } = 1024 * 16;

        public void Close()
        {
            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Initiated Close", CancellationToken.None);
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, CancellationToken cancellationToken)
        {
            await socket.SendAsync(buffer, messageType, true, cancellationToken);
        }

        public async Task<string> ReceiveAsync()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[BufferSize]);
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
        /// Provides new client instance or null if it is not socket.
        /// </summary>
        Task<IClient> ProcessClientRequest(CancellationToken token);
    }

    /// <summary>
    /// Wrapper around httpListener.
    /// </summary>
    public class ExtendedHttpListener : IListener
    {
        private readonly int buffer;
        private readonly HttpListener httpListener;

        public ExtendedHttpListener(IPEndPoint endPoint, int buffer)
        {
            this.buffer = buffer;
            httpListener =  new HttpListener();
            httpListener.Prefixes.Add(endPoint.ToString());
        }

        public void Start() => httpListener.Start();
        public void Stop() => httpListener.Stop();

        public async Task<IClient> ProcessClientRequest(CancellationToken token)
        {
            HttpListenerContext hc = await httpListener.GetContextAsync();

            if (hc.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext ws = await hc.AcceptWebSocketAsync(null);
                
                return new SocketClient(ws.WebSocket);
            }
            else if (hc.Request.HttpMethod == "GET")
            {

            }

            return null;
        }
    }
}