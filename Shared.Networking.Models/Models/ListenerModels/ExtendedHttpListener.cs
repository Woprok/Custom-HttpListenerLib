using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Networking.Models.Interfaces.ListenerModels;

namespace Shared.Networking.Models.Models.ListenerModels
{
    /// <summary>
    /// Wrapper around httpListener.
    /// </summary>
    public class ExtendedHttpListener : IListener
    {
        private readonly HttpListener httpListener;

        public ExtendedHttpListener(IPEndPoint endPoint)
        {
            httpListener =  new HttpListener();
            httpListener.Prefixes.Add(endPoint.ToString());
        }

        public void Start() => httpListener.Start();
        public void Stop() => httpListener.Stop();

        public async Task<ISocketClient> ProcessClientRequest(CancellationToken token)
        {
            HttpListenerContext hc = await httpListener.GetContextAsync();

            if (hc.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext ws = await hc.AcceptWebSocketAsync(null);
                
                return new SocketClient(ws.WebSocket);
            }
            else if (hc.Request.HttpMethod == "GET")
            {

            }

            return null;
        }
    }
}