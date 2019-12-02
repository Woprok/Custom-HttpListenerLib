using System.IO;
using System.Net;

namespace Shared.Networking
{
    /// <summary>
    /// Provides wrapper interface for client.
    /// </summary>
    public interface IClient
    {
        Stream GetStream();
        bool Connected { get; set; }
        int ReceiveBufferSize { get; set; }
        int SendBufferSize { get; set; }
        void Close();
    }

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
        /// Provides new client instance.
        /// </summary>
        IClient AcceptClientAsync();
    }

    /// <summary>
    /// Wrapper around httpListener.
    /// </summary>
    public class ExtendedHttpListener : IListener
    {
        private readonly HttpListener httpListener;
        public ExtendedHttpListener(IPEndPoint endPoint)
        {
            httpListener =  new HttpListener();
        }

        public void Start() => httpListener.Start();
        public void Stop() => httpListener.Stop();
        public IClient AcceptClientAsync() => null;
    }
}