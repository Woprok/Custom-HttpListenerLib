using System.Net;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Base of upper layer that manages data sending& receiving.
    /// </summary>
    public interface IDataModel
    {
        /// <summary>
        /// Desired amount of bytes buffered in TcpClient
        /// </summary>
        int BufferSize { get; }

        /// <inheritdoc cref="IPEndPoint"/>
        IPEndPoint IpEndPoint { get; }
    }
}