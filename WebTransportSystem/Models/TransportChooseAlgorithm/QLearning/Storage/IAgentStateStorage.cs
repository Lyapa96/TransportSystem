namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage
{
    public interface IAgentStateStorage
    {
        TransportType GetBestNextTransport(AgentState agentState);
        void SaveStateReward(string agentState, TransportType transportType, double reward);
    }
}