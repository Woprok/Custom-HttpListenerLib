using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.ListenerModels
{
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
        Task<ISocketClient> ProcessClientRequest(CancellationToken token);
    }
}