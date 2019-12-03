using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Enums;
using Shared.Common.Exceptions;
using Shared.Common.Models;
using Shared.Networking.Models.Interfaces;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="StartStopModel"/>
    /// <inheritdoc cref="IServerModel"/>
    public sealed class ServerModel : StartStopModel, IServerModel
    {
        /// <inheritdoc/>
        public event ClientObtained OnNewClient;

        public ServerModel() : base() { }

        public ServerModel(IPEndPoint ipEndPoint) : this()
        {
            IpEndPoint = ipEndPoint;
        }

        /// <inheritdoc/>
        public IPEndPoint IpEndPoint { get; set; }

        /// <inheritdoc/>
        public IListener Listener { get; private set; }

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            //Tcp client accepted IpEndPoint, HttpListener uses Prefixes
            Listener = new ExtendedHttpListener(IpEndPoint);
        }

        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Listener.Start();
            Task.Factory.StartNew(() => AcceptConnectionAsync(CurrentCancellationToken), CurrentCancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <inheritdoc/>
        protected override void OnModelStop()
        {
            Listener.Stop();
        }

        private async Task AcceptConnectionAsync(CancellationToken token)
        {
            while (InternalModelState == ModelState.Started && !token.IsCancellationRequested)
            {
                IClient obtainedClient = await Listener.ProcessClientRequest(token);

                if (obtainedClient == null) //It is not socket
                    continue;

                if (token.IsCancellationRequested || InternalModelState == ModelState.Stopped)
                    return;

                if (OnNewClient == null)
                    throw new EventNotSubscribedException(nameof(AcceptConnectionAsync));
                await Task.Run(() => OnNewClient?.Invoke(obtainedClient), CurrentCancellationToken);
            }
        }
    }
}