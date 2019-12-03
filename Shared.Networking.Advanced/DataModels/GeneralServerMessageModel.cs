using System;
using System.Net;
using Shared.Networking.Advanced.Entities.Messages;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models;

namespace Shared.Networking.Advanced.DataModels
{
    /// <summary>
    /// Model that controls and coordinates receiving and sending of the messages.
    /// Responsible for parsing and wrapping data by message type.
    /// </summary>
    public class GeneralServerMessageModel : GenericDataModel<object, ServerModel>
    {
        public GeneralServerMessageModel(IPEndPoint ipEndPoint, ISerializer serializer, int defaultBufferSize = DefaultBufferSize, bool autorun = true) 
            : base(ipEndPoint, serializer, defaultBufferSize, autorun) { }
        
        public override void DataExchangerDataReceived(ISendReceiveModel exchangerModel, CoreMessage data)
        {
            Console.WriteLine(data);
            base.DataExchangerDataReceived(exchangerModel, data);
        }

        public override void DataExchangerDataSent(ISendReceiveModel sender, CoreMessage data)
        {
            base.DataExchangerDataSent(sender, data);
        }

        public override void DataExchangerDisconnected(ISendReceiveModel exchangerModel)
        {
            base.DataExchangerDisconnected(exchangerModel);
        }
    }
}