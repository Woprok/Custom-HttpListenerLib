using System.Net;

namespace Shared.Networking
{
    /// <summary>
    /// Wrapper around httpListener.
    /// </summary>
    public class ExtendedHttpListener
    {
        private readonly HttpListener httpListener;
        public ExtendedHttpListener(IPEndPoint endPoint)
        {
            httpListener =  new HttpListener();
        }

        public void Start() => httpListener.Start();
        public void Stop() => httpListener.Stop();
        public void AcceptSocketAsync() => null;
    }
}