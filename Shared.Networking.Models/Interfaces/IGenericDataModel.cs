using System.Collections.Generic;
using System.Collections.Immutable;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Upper layer of connection.
    /// Every client is required to established validated connection asap.
    /// Non-validated clients are ignored down the path.
    /// </summary>
    /// <typeparam name="TConnector">Type of connection, either Client or Server</typeparam>
    public interface IGenericDataModel<TConnector> where TConnector : class, IConnector, new()
    {
        /// <inheritdoc cref="IConnector"/>
        TConnector Model { get; }

        /// <inheritdoc cref="ISerializer"/>
        ISerializer Serializer { get; }

        /// <summary>
        /// Collection of all current connections.
        /// </summary>
        Dictionary<long, ISendReceiveModel> DataExchangers { get; set; }

        /// <summary>
        /// Sends message/data to all registered validated ISendReceiveModels.
        /// </summary>
        void SendToAllValidConnections(object message);

        /// <summary>
        /// Sends message/data to all registered ISendReceiveModels.
        /// </summary>
        void SendToAll(object message);

        /// <summary>
        /// Obtain all current exchangers as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel> GetExchangers();

        /// <summary>
        /// Obtain all connected and validated exchangers as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel> GetValidatedExchangers();
        
        /// <summary>
        /// Obtain all connected and validated exchangers that are subset of ids as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel> GetValidatedExchangerByIds(HashSet<long> ids);

        /// <summary>
        /// Handles method that manages connections.
        /// </summary>
        void DataExchangerDisconnected(ISendReceiveModel exchangerModel);

        /// <summary>
        /// Handles method that manages sent data.
        /// </summary>
        void DataExchangerDataSent(ISendReceiveModel sender, object data);

        /// <summary>
        /// Handles method that manages received data.
        /// </summary>
        void DataExchangerDataReceived(ISendReceiveModel exchangerModel, object data);

        /// <summary>
        /// Handles method that manages error propagation.
        /// </summary>
        void DataExchangerError(ISendReceiveModel receiver);

        /// <summary>
        /// Finishes initialization and starts running.
        /// This is shortcut for calling Initialize() & Start()
        /// </summary>
        void StartRunning();
    }
}