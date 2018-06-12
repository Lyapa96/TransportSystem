using System.Collections.Generic;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning
{
    public class QLearningFunc : ITransmissionFunc
    {
        private readonly IAgentStateStorage stateStorage;

        public QLearningFunc(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public TransportType ChooseNextTransportType(
            HashSet<Passenger> neighbors,
            TransportType currentTransportType,
            double currentSatisfaction,
            double deviationValue)
        {
            var agentState = new AgentState(neighbors, currentSatisfaction, currentTransportType);
            return stateStorage.GetBestNextTransport(agentState);
        }
    }
}