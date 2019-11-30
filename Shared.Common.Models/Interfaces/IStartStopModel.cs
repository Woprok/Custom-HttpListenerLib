namespace Shared.Common.Models.Interfaces
{
    /// <summary>
    /// Simple model for wrapping Initialize, Start and Stop operations, while securing thread safety of these methods.
    /// </summary>
    public interface IStartStopModel
    {
        /// <summary>
        /// Calls onModelInitialize.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Calls OnModelStart and initialize new TokenSource.
        /// </summary>
        void Start();

        /// <summary>
        /// Calls OnModelStop and do TokenSource.Cancel.
        /// </summary>
        void Stop();
    }
}