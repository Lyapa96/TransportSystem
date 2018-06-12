using System.Collections.Generic;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class TransmissionFuncFactory
    {
        private readonly Dictionary<TransmissionType, ITransmissionFunc> typeToFunc;

        public TransmissionFuncFactory(IAgentStateStorage stateStorage)
        {
            typeToFunc = new Dictionary<TransmissionType, ITransmissionFunc>
            {
                {TransmissionType.Average, new AveragingFunc()},
                {TransmissionType.Deviation, new DeviationFunc()},
                {TransmissionType.QLearning, new QLearningFunc(stateStorage)}
            };
        }

        public ITransmissionFunc GetTransmissionFunc(TransmissionType type)
        {
            return typeToFunc[type];
        }
    }
}