using System.Collections.Generic;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class TransmissionFuncFactory
    {
        private Dictionary<TransmissionType, ITransmissionFunc> typeToFunc =
            new Dictionary<TransmissionType, ITransmissionFunc>()
            {
                {TransmissionType.Average, new AveragingFunc()},
                {TransmissionType.Deviation, new DeviationFunc()}
            };

        public ITransmissionFunc GetTransmissionFunc(TransmissionType type)
        {
            return typeToFunc[type];
        }
    }
}