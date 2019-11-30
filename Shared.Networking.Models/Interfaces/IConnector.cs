using System.Net;
using System.Net.Sockets;
using Shared.Common.Models.Interfaces;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Default event for notifying of new client connection.
    /// </summary>
    public delegate void ClientObtained(TcpClient newClient);

    /// <summary>
    /// Simple interface for managing client-server (IClientModel-IServerModel) connection.
    /// </summary>
    public interface IConnector : IStartStopModel
    {
        /// <summary>
        /// Subscribe-able event used to obtain new client.
        /// </summary>
        event ClientObtained OnNewClient;

        /// <inheritdoc cref="IPEndPoint"/>
        IPEndPoint IpEndPoint { get; set; }
    }
}