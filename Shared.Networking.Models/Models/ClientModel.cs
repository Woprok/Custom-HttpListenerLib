using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shared.Common.Exceptions.Exceptions;
using Shared.Common.Models.Enums;
using Shared.Common.Models.Models;
using Shared.Networking.Models.Interfaces;

namespace Shared.Networking.Models.Models
{
    /// <inheritdoc cref="StartStopModel"/>
    /// <inheritdoc cref="IClientModel"/>
    public sealed class ClientModel : StartStopModel, IClientModel
    {
        /// <inheritdoc/>
        public event ClientObtained OnNewClient;
        
        public ClientModel() : base() { } 

        public ClientModel(IPEndPoint ipEndPoint) : this()
        {
            IpEndPoint = ipEndPoint;
        }

        /// <inheritdoc/>
        public IPEndPoint IpEndPoint { get; set; }

        /// <inheritdoc/>
        public TcpClient Client { get; private set; }

        /// <inheritdoc/>
        protected override void OnModelInitialize()
        {
            Client = new TcpClient();
        }

        /// <inheritdoc/>
        protected override void OnModelStart()
        {
            Task.Factory.StartNew(() => ConnectAsync(CurrentCancellationToken), CurrentCancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <inheritdoc/>
        protected override void OnModelStop()
        {
            Client.Close();
        }

        private async Task ConnectAsync(CancellationToken token)
        {
            await Client.ConnectAsync(IpEndPoint.Address, IpEndPoint.Port);
            if (token.IsCancellationRequested || InternalModelState == ModelState.Stopped)
                return;

            if (OnNewClient == null)
                throw new EventNotSubscribedException(nameof(ConnectAsync));
            await Task.Run(() => OnNewClient?.Invoke(Client), CurrentCancellationToken);
        }
    }
}