using System.Threading;
using Shared.Common.Exceptions.Exceptions;

namespace Shared.Common.Models.Models
{
    /// <summary>
    /// Provides base objects and methods for multi-threading and working with async methods.
    /// </summary>
    public abstract class ThreadModel
    {
        /// <inheritdoc cref="CancellationTokenSource"/>
        private CancellationTokenSource tokenSource;
        
        /// <summary>
        /// Provides default object for synchronization for more complex models.
        /// </summary>
        protected readonly object CriticalAccessLock = new object();

        /// <summary>
        /// Get current CancellationTokenSource instance. 
        /// </summary>
        /// <exception cref="InvalidCallException">On attempt to get not existing token.</exception>
        protected CancellationToken CurrentCancellationToken => tokenSource?.Token ?? throw new InvalidCallException(nameof(CurrentCancellationToken));

        /// <summary>
        /// Creates new instance of CancellationTokenSource.
        /// </summary>
        /// <exception cref="InvalidCallException">On attempt to create new over existing token.</exception>
        protected void CreateToken() => tokenSource = tokenSource == null ? new CancellationTokenSource() : throw new InvalidCallException(nameof(CreateToken));

        /// <summary>
        /// Cancel this model CancellationTokenSource.
        /// </summary>
        protected void CancelToken() => tokenSource.Cancel();

        /// <summary>
        /// Disposes this model CancellationTokenSource.
        /// </summary>
        protected void DisposeToken() => tokenSource.Dispose();
    }
}