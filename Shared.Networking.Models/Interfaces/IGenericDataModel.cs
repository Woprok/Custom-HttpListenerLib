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
    /// <typeparam name="T">Type that is being exchanged between client-server</typeparam>
    /// <typeparam name="TConnector">Type of connection, either Client or Server</typeparam>
    public interface IGenericDataModel<T, TConnector> where TConnector : class, IConnector, new()
    {
        /// <inheritdoc cref="IConnector"/>
        TConnector Model { get; }

        /// <inheritdoc cref="ISerializer{T}"/>
        ISerializer<T> Serializer { get; }

        /// <summary>
        /// Collection of all current connections.
        /// </summary>
        Dictionary<long, ISendReceiveModel<T>> DataExchangers { get; set; }

        /// <summary>
        /// Sends message/data to all registered validated ISendReceiveModels.
        /// </summary>
        void SendToAllValidConnections(T message);

        /// <summary>
        /// Sends message/data to all registered ISendReceiveModels.
        /// </summary>
        void SendToAll(T message);

        /// <summary>
        /// Obtain all current exchangers as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel<T>> GetExchangers();

        /// <summary>
        /// Obtain all connected and validated exchangers as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel<T>> GetValidatedExchangers();
        
        /// <summary>
        /// Obtain all connected and validated exchangers that are subset of ids as ImmutableCollection.
        /// </summary>
        ImmutableDictionary<long, ISendReceiveModel<T>> GetValidatedExchangerByIds(HashSet<long> ids);

        /// <summary>
        /// Handles method that manages connections.
        /// </summary>
        void DataExchangerDisconnected(ISendReceiveModel<T> exchangerModel);

        /// <summary>
        /// Handles method that manages sent data.
        /// </summary>
        void DataExchangerDataSent(ISendReceiveModel<T> sender, T data);

        /// <summary>
        /// Handles method that manages received data.
        /// </summary>
        void DataExchangerDataReceived(ISendReceiveModel<T> exchangerModel, T data);

        /// <summary>
        /// Finishes initialization and starts running.
        /// This is shortcut for calling Initialize() & Start()
        /// </summary>
        void StartRunning();
    }
}