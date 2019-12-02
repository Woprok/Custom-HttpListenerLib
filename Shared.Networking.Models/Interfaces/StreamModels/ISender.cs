using System.Threading;
using System.Threading.Tasks;

namespace Shared.Networking.Models.Interfaces.StreamModels
{
    /// <summary>
    /// Default event for publishing sent data.
    /// </summary>
    public delegate void DataSent<T>(ISender sender, T data);

    /// <summary>
    /// Standard interface for data sending.
    /// </summary>
    public interface ISender : ISerializationModel
    {
        /// <summary>
        /// Subscribe-able event for sent data.
        /// </summary>
        event DataSent<object> OnDataSent;
        
        /// <summary>
        /// Async method for sending object of type T.
        /// </summary>
        Task SendAsync<TE>(TE item, CancellationToken token);
    }
}