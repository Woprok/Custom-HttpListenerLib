using System.Net;
using Shared.Networking.Advanced.Entities.Messages;
using Shared.Networking.Models.Interfaces;
using Shared.Networking.Models.Interfaces.StreamModels;
using Shared.Networking.Models.Models;

namespace Shared.Networking.Advanced.DataModels
{
    public class GeneralClientMessageModel : GenericDataModel<CoreMessage, ClientModel>
    {
        public GeneralClientMessageModel(IPEndPoint ipEndPoint, ISerializer<CoreMessage> serializer, int defaultBufferSize = DefaultBufferSize, bool autorun = true) : base(ipEndPoint, serializer, defaultBufferSize, autorun)
        {

        }

        public override void DataExchangerDataReceived(ISendReceiveModel<CoreMessage> exchangerModel, CoreMessage data)
        {
            base.DataExchangerDataReceived(exchangerModel, data);
        }

        public override void DataExchangerDataSent(ISendReceiveModel<CoreMessage> sender, CoreMessage data)
        {
            base.DataExchangerDataSent(sender, data);
        }

        public override void DataExchangerDisconnected(ISendReceiveModel<CoreMessage> exchangerModel)
        {
            base.DataExchangerDisconnected(exchangerModel);
        }
    }
}