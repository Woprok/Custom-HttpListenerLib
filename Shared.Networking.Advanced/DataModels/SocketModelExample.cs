﻿using System;
using System.Net;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models;
using Shared.Networking.Models.Models.ListenerModels;

namespace Shared.Networking.Advanced.DataModels
{
    /// <summary>
    /// Model that controls and coordinates receiving and sending of the messages.
    /// Responsible for parsing and wrapping data by message type.
    /// </summary>
    public class SocketModelExample : GenericDataModel<ServerModel>
    {
        public SocketModelExample(string ipEndPoint, ISerializer serializer, int defaultBufferSize = DefaultBufferSize, bool autorun = true) 
            : base(ipEndPoint, serializer, defaultBufferSize, autorun) { }
        
        public override void DataExchangerDataReceived(ISendReceiveModel exchangerModel, object data)
        {
            Console.WriteLine(data);
            base.DataExchangerDataReceived(exchangerModel, data);
        }

        public override void DataExchangerDataSent(ISendReceiveModel sender, object data)
        {
            base.DataExchangerDataSent(sender, data);
        }

        public override void DataExchangerDisconnected(ISendReceiveModel exchangerModel)
        {
            base.DataExchangerDisconnected(exchangerModel);
        }

        public override void DataExchangerError(ISendReceiveModel receiver)
        {
            base.DataExchangerError(receiver);
        }
    }
}