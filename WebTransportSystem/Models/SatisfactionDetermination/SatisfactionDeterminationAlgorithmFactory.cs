using System.Collections.Generic;
using WebTransportSystem.Models.TransportChooseAlgorithm;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.SatisfactionDetermination
{
    public class SatisfactionDeterminationAlgorithmFactory
    {
        private readonly Dictionary<TransmissionType, ISatisfactionDeterminationAlgorithm> typeToAlgorithm;

        public SatisfactionDeterminationAlgorithmFactory(IAgentStateStorage storage)
        {
            typeToAlgorithm = new Dictionary<TransmissionType, ISatisfactionDeterminationAlgorithm>
            {
                {TransmissionType.Average, new LastFiveTripsAlgorithm()},
                {TransmissionType.Deviation, new LastFiveTripsAlgorithm()},
                {TransmissionType.QLearning, new QLearningQualityCoefficientAlgorithm(storage)}
            };
        }

        public ISatisfactionDeterminationAlgorithm GetAlgoritm(TransmissionType transmissionType)
        {
            return typeToAlgorithm[transmissionType];
        }
    }
}