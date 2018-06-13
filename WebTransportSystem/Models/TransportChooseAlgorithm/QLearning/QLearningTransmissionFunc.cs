using System.Collections.Generic;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning
{
    public class QLearningTransmissionFunc : ITransmissionFunc
    {
        private readonly IAgentStateStorage stateStorage;

        public QLearningTransmissionFunc(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue)
        {
            var currentAgentState = new AgentState(neighbors, currentSatisfaction, currentTransportType).GetStringFormat();
            return stateStorage.GetBestNextTransport(currentAgentState);
        }
    }
}