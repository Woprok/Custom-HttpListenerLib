using System.Net.Sockets;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Provides required minimum for client part of connection.
    /// </summary>
    /// <inheritdoc cref="IConnector"/>
    public interface IClientModel : IConnector
    {
        /// <inheritdoc cref="TcpClient"/>
        TcpClient Client { get; }
    }
}