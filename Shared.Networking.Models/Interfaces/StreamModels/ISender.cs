using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Default event for publishing sent data on success.
    /// </summary>
    public delegate void DataSentSuccess(ISender sender, object data);
    /// <summary>
    /// Default event for publishing sent data failure.
    /// </summary>
    public delegate void DataSentError(ISender sender, Exception e, object data);

    /// <summary>
    /// Standard interface for data sending.
    /// </summary>
    public interface ISender : ISerializationModel
    {
        /// <summary>
        /// Subscribe-able event for sent data.
        /// </summary>
        event DataSentSuccess OnDataSentSuccess;
        /// <summary>
        /// Subscribe-able event for sent data failure.
        /// </summary>
        event DataSentError OnDataSentError;
        /// <summary>
        /// Async method for sending object of type T.
        /// </summary>
        Task SendAsync<TE>(TE item, CancellationToken token);
    }
}