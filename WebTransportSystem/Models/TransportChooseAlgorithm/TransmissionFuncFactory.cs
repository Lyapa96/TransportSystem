using System.Collections.Generic;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class TransmissionFuncFactory
    {
        private const double CarAvailabilityProbability = 85;
        private readonly Dictionary<TransmissionType, ITransmissionFunc> typeToFunc;

        public TransmissionFuncFactory(IAgentStateStorage stateStorage)
        {
            typeToFunc = new Dictionary<TransmissionType, ITransmissionFunc>
            {
                {TransmissionType.Average, new AveragingFunc(CarAvailabilityProbability)},
                {TransmissionType.Deviation, new DeviationFunc()},
                {TransmissionType.QLearning, new QLearningTransmissionFunc(stateStorage)}
            };
        }

        public ITransmissionFunc GetTransmissionFunc(TransmissionType type)
        {
            return typeToFunc[type];
        }
    }
}