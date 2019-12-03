using System.Net;
using Shared.Common.Interfaces;

namespace Shared.Networking.Models.Interfaces.ListenerModels
{
    /// <summary>
    /// Default event for notifying of new client connection.
    /// </summary>
    public delegate void ClientObtained(ISocketClient newSocketClient);

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
        string IpEndPoint { get; set; }
    }
}