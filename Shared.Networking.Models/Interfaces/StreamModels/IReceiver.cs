using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Default event for publishing received data.
    /// </summary>
    public delegate void DataReceived<T>(IReceiver<T> receiver, T data);

    /// <summary>
    /// Default event for publishing client disconnected status.
    /// </summary>
    public delegate void ClientDisconnected<T>(IReceiver<T> receiver);

    /// <summary>
    /// Standard interface for data receiving.
    /// </summary>
    public interface IReceiver<T> : ISerializationModel<T>
    {
        /// <summary>
        /// Subscribe-able event used to obtain received data.
        /// </summary>
        event DataReceived<T> OnDataReceived;

        /// <summary>
        /// Subscribe-able event used for obtaining client disconnected status.
        /// </summary>
        event ClientDisconnected<T> OnClientDisconnected;
        
        /// <summary>
        /// Async method for receiving object of type T.
        /// </summary>
        Task ReceiveAsync(CancellationToken token);
    }
}