using System.Collections.Generic;
using MongoDB.Bson;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning
{
    public class MongoAgentStateInfo
    {
        public ObjectId Id { get; set; }
        public string State { get; set; }
        public List<AgentAction> AgentActions { get; set; } 
    }
}