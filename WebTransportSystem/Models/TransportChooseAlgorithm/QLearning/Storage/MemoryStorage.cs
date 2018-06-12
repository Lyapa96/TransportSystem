using System.Collections.Generic;
using System.Linq;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage
{
    public class MemoryStorage : IAgentStateStorage
    {
        private readonly Dictionary<string, List<QLearningTransportTypeInfo>> agentStateToTransportTypes =
            new Dictionary<string, List<QLearningTransportTypeInfo>>();

        public TransportType GetBestNextTransport(AgentState agentState)
        {
            var stringAgentState = agentState.GetAsString();
            if (agentStateToTransportTypes.ContainsKey(stringAgentState))
            {
                var allTransportTypes = agentStateToTransportTypes[stringAgentState];
                return allTransportTypes
                    .OrderByDescending(x => x.GetAverageValue)
                    .First()
                    .TransportType;
            }

            return PassengersHelper.GetRandomtransportType();
        }

        public void SaveStateReward(string agentState, TransportType transportType, double reward)
        {
            if (agentStateToTransportTypes.ContainsKey(agentState))
            {
                var allTransportTypes = agentStateToTransportTypes[agentState];
                var current = allTransportTypes.Find(x => x.TransportType == transportType);
                current.Rewards.Add(reward);
            }
            else
            {
                agentStateToTransportTypes[agentState] = StorageHelpers.CreateFirstInfo(transportType, reward);
            }
        }
    }
}