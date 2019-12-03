using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Default event for publishing received data on success.
    /// </summary>
    public delegate void DataReceivedSuccess(IReceiver receiver, object data);
    /// <summary>
    /// Default event for publishing received data on failure.
    /// </summary>
    public delegate void DataReceivedError(IReceiver receiver, Exception e);
    /// <summary>
    /// Default event for publishing client disconnected status.
    /// </summary>
    public delegate void ClientDisconnected(IReceiver receiver);

    /// <summary>
    /// Standard interface for data receiving.
    /// </summary>
    public interface IReceiver : ISerializationModel
    {
        /// <summary>
        /// Subscribe-able event used to obtain received data.
        /// </summary>
        event DataReceivedSuccess OnDataReceivedSuccess;
        /// <summary>
        /// Subscribe-able event used to obtain information about failed received data.
        /// </summary>
        event DataReceivedError OnDataReceivedError;

        /// <summary>
        /// Subscribe-able event used for obtaining client disconnected status.
        /// </summary>
        event ClientDisconnected OnClientDisconnected;
        
        /// <summary>
        /// Async method for receiving object of type T.
        /// </summary>
        Task ReceiveAsync(CancellationToken token);
    }
}