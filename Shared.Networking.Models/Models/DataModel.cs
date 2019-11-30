using System.Net;
using Shared.Common.Models.Models;
using Shared.Networking.Models.Interfaces;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="StartStopModel"/>
    /// <inheritdoc cref="IDataModel"/>
    public abstract class DataModel : StartStopModel, IDataModel
    {
        protected const int DefaultBufferSize = 1024 * 16;

        protected DataModel() : base() { }
        protected DataModel(IPEndPoint ipEndPoint, int defaultBufferSize = DefaultBufferSize) : this()
        {
            IpEndPoint = ipEndPoint;
            BufferSize = defaultBufferSize;
        }
        
        /// <inheritdoc/>
        public int BufferSize { get; } = DefaultBufferSize;
        
        /// <inheritdoc/>
        public IPEndPoint IpEndPoint { get; }
    }
}