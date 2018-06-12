using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.SatisfactionDetermination
{
    public class QLearningQualityCoefficientAlgorithm : ISatisfactionDeterminationAlgorithm
    {
        private readonly IAgentStateStorage stateStorage;

        public QLearningQualityCoefficientAlgorithm(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public double GetSatisfaction(Passenger passenger)
        {
            var qualityCoefficient = passenger.QualityCoefficient;
            stateStorage.SaveStateReward(passenger.PreviousState, passenger.TransportType, qualityCoefficient);

            return qualityCoefficient;
        }
    }
}