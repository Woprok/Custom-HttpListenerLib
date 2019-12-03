using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.ListenerModels
{
    /// <summary>
    /// Provides wrapper interface for client.
    /// </summary>
    public interface ISocketClient
    {
        bool Connected { get; }
        int BufferSize { get; set; }
        void Close();
        Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, CancellationToken cancellationToken);
        Task<string> ReceiveAsync();
    }
}