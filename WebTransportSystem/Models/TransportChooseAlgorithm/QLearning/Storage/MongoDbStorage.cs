using System.Linq;
using MongoDB.Driver;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage
{
    public class MongoDbStorage : IAgentStateStorage
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "TransportSystem";
        private const string CollectionName = "AgentStates";
        private readonly IMongoCollection<MongoAgentStateInfo> collection;

        public MongoDbStorage()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            collection = database.GetCollection<MongoAgentStateInfo>(CollectionName);
            if (collection == null)
            {
                database.CreateCollection(CollectionName);
                collection = database.GetCollection<MongoAgentStateInfo>(CollectionName);
            }
        }

        public TransportType GetBestNextTransport(AgentState agentState)
        {
            var findFluent = collection.Find(x => x.State == agentState.GetAsString()).FirstOrDefault();
            if (findFluent == null)
            {
                return PassengersHelper.GetRandomtransportType();
            }

            var best = findFluent.AgentActions.First();
            foreach (var action in findFluent.AgentActions)
            {
                if (action.AverageReward >= best.AverageReward)
                    best = action;
            }

            return best.Transport.ToEnum<TransportType>();
        }

        public void SaveStateReward(string agentState, TransportType transportType, double reward)
        {
            var item = collection.Find(x => x.State == agentState).FirstOrDefault();
            if (item != null)
            {
                var allTransportTypes = item.AgentActions;
                var current = allTransportTypes.Find(x => x.Transport == transportType.ToString());
                current.AddReward(reward);
                var updateDef = Builders<MongoAgentStateInfo>.Update.Set(o => o.AgentActions, allTransportTypes);
                collection.UpdateOne(x => x.State == agentState, updateDef);
            }
            else
            {
                collection.InsertOne(
                    new MongoAgentStateInfo
                    {
                        State = agentState,
                        AgentActions = StorageHelpers.CreateFirstAgentActions(transportType, reward)
                    });
            }
        }
    }
}