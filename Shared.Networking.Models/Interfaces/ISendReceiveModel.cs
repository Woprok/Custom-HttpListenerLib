using System.Net.Sockets;
using Shared.Common.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;

namespace Shared.Networking.Models.Interfaces
{
    /// <summary>
    /// Default event for publishing client disconnected status.
    /// </summary>
    public delegate void SendReceiveModelDisconnected<T>(ISendReceiveModel receiver);

    /// <summary>
    /// Default event for publishing sent data.
    /// </summary>
    public delegate void SendReceiveModelDataSent<T>(ISendReceiveModel sender, T data);
    
    /// <summary>
    /// Default event for publishing received data.
    /// </summary>
    public delegate void SendReceiveModelDataReceived<T>(ISendReceiveModel receiver, T data);

    /// <summary>
    /// United model for receiving and sending data of type T.
    /// </summary>
    /// <inheritdoc cref="IStartStopModel"/>
    /// <inheritdoc cref="ISerializationModel"/>
    public interface ISendReceiveModel : IStartStopModel, ISerializationModel
    {
        /// <summary>
        /// Subscribe-able event used for obtaining client disconnected status.
        /// </summary>
        event SendReceiveModelDisconnected<object> OnSendReceiveModelDisconnected;
        
        /// <summary>
        /// Subscribe-able event for sent data.
        /// </summary>
        event SendReceiveModelDataSent<object> OnSendReceiveModelDataSent;

        /// <summary>
        /// Subscribe-able event used to obtain received data.
        /// </summary>
        event SendReceiveModelDataReceived<object> OnSendReceiveModelDataReceived;

        /// <summary>
        /// Provides unique identifier for this SendReceiveModel.
        /// </summary>
        long Id { get; }

        /// <inheritdoc cref="IClient"/>
        IClient Client { get; }

        /// <inheritdoc cref="IReceiver"/>
        IReceiver Receiver { get; }
        
        /// <inheritdoc cref="ISender"/>
        ISender Sender { get; }

        /// <summary>
        /// Method for sending object of type T.
        /// Implementation of this function should be async.
        /// </summary>
        void Send(object obj);

        /// <summary>
        /// Holds information about authentication.
        /// Default state is false.
        /// Once account is authenticated, this value will be set to true.
        /// </summary>
        bool IsValidated { get; set; }

        /// <summary>
        /// Returns IsValidated && Client.Connected.
        /// </summary>
        bool IsValidConnection { get; }
    }
}