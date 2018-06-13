using System.Collections.Generic;
using System.Linq;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage
{
    public class MemoryStorage : IAgentStateStorage
    {
        private readonly Dictionary<string, List<QLearningTransportTypeInfo>> agentStateToTransportTypes =
            new Dictionary<string, List<QLearningTransportTypeInfo>>();

        public TransportType GetBestNextTransport(string currentAgentState)
        {
            if (agentStateToTransportTypes.ContainsKey(currentAgentState))
            {
                var allTransportTypes = agentStateToTransportTypes[currentAgentState];
                return allTransportTypes
                    .OrderByDescending(x => x.GetAverageValue)
                    .First()
                    .TransportType;
            }

            return PassengersHelper.GetRandomtransportType();
        }

        public void SaveStateReward(string previousAgentState, string currentAgentState, double reward, TransportType previousAction)
        {
            if (agentStateToTransportTypes.ContainsKey(previousAgentState))
            {
                var allTransportTypes = agentStateToTransportTypes[previousAgentState];
                var current = allTransportTypes.Find(x => x.TransportType == previousAction);
                current.Rewards.Add(reward);
            }
            else
            {
                agentStateToTransportTypes[previousAgentState] = StorageHelpers.CreateFirstInfo(previousAction, reward);
            }
        }
    }
}