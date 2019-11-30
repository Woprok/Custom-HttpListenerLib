using System.IO;
using System.Net.Sockets;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Provides shared methods for Sending and Receiving over TcpClient.
    /// </summary>
    public interface IDataStreamModel
    {
        /// <summary>
        /// Returns TcpClient currently in use by model.
        /// </summary>
        TcpClient Client { get; }

        /// <summary>
        /// Returns TcpClient data stream.
        /// </summary>
        Stream ClientStream { get; }

        /// <summary>
        /// Returns current connection state of TcpClient.
        /// </summary>
        bool IsConnected { get; }
    }
}