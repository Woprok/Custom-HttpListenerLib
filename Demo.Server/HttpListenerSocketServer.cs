using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Server
{
    /// <summary>
    /// https://stackoverflow.com/questions/24239953/wcf-self-hosted-websocket-service-with-javascript-client
    /// </summary>
    public class HttpListenerSocketServer
    {
        #region Fields

        private static CancellationTokenSource m_cancellation;
        private static HttpListener m_listener;

        #endregion

        #region Private Methods

        private static async Task AcceptWebSocketClientsAsync(HttpListener server, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                HttpListenerContext hc = await server.GetContextAsync();

                Console.WriteLine("Request: " + hc.Request.Headers.AllKeys);
                Console.WriteLine("Response: " + hc.Response.StatusCode);

                if (hc.Request.HttpMethod == "GET")
                {
                    Console.WriteLine(hc.Request.Url);
                }

                if (!hc.Request.IsWebSocketRequest)
                {
                    hc.Response.StatusCode = 400;
                    hc.Response.Close();
                    continue;
                }


                try
                {
                    HttpListenerWebSocketContext ws = await hc.AcceptWebSocketAsync(null).ConfigureAwait(false);
                    if (ws != null)
                    {
                        Task.Run(() => HandleConnectionAsync(ws.WebSocket, token));
                    }
                }
                catch (Exception aex)
                {
                    // Log error here
                }
            }
        }

        private static async Task HandleConnectionAsync(WebSocket ws, CancellationToken cancellation)
        {
            try
            {
                while (ws.State == WebSocketState.Open && !cancellation.IsCancellationRequested)
                {
                    string messageString = await ReadString(ws).ConfigureAwait(false);
                    Console.WriteLine("Received:" + messageString);
                    
                    string strReply = "OK"; // Process messageString and get your reply here;
                    byte[] buffer = Encoding.UTF8.GetBytes(strReply);
                    ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                    await ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None)
                        .ConfigureAwait(false);
                }

                Console.WriteLine("Close: ");
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
            }
            catch (Exception aex)
            {
                // Log error
                Console.WriteLine("Error: " + aex.Message);

                try
                {
                    await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Done", CancellationToken.None).ConfigureAwait(false);
                }
                catch
                {
                    // Do nothing
                }
            }
            finally
            {
                ws.Dispose();
            }
        }

        private static async Task<string> ReadString(WebSocket ws)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);

            WebSocketReceiveResult result = null;

            using (MemoryStream ms = new MemoryStream())
            {
                do
                {
                    result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion

        #region Public Methods

        public async void Start(string uri)
        {
            Console.WriteLine("Creating host.");

            m_listener = new HttpListener();
            m_listener.Prefixes.Add(uri);
            m_listener.Start();
            m_cancellation = new CancellationTokenSource();
            await Task.Run(() => AcceptWebSocketClientsAsync(m_listener, m_cancellation.Token));

            Console.WriteLine("Closing host.");
        }

        public void Stop()
        {
            if (m_listener != null && m_cancellation != null)
            {
                try
                {
                    m_cancellation.Cancel();

                    m_listener.Stop();

                    m_listener = null;
                    m_cancellation = null;
                }
                catch
                {
                    // Log error
                }
            }
        }

        #endregion
    }
}