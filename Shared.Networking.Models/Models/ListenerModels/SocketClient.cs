using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared.Networking.Models.Interfaces.ListenerModels;

namespace Shared.Networking.Models.Models.ListenerModels
{
    public class SocketClient : ISocketClient
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
}