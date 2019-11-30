using System.Net;
using System.Net.Sockets;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Provides required minimum for server part of connection.
    /// Use as follow:
    /// Call Initialize -> Subscribe OnClientCreated -> Call Start
    /// If needed it should be possible to UnSubscribe OnClientCreated -> Call Stop
    /// </summary>
    /// <inheritdoc cref="IConnector"/>
    public interface IServerModel : IConnector
    {
        /// <inheritdoc cref="HttpListener"/>
        ExtendedHttpListener Listener { get; }
    }
}