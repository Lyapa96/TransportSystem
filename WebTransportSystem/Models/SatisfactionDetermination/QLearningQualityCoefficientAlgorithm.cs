using System;
using System.Linq;
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
            var curretnQualityCoefficient = passenger.QualityCoefficient;
            var reward = 0.0;
            if (passenger.AllQualityCoefficients.Count != 0)
            {
                var previousQualityCoefficient = passenger.AllQualityCoefficients.Last();
                reward = previousQualityCoefficient > curretnQualityCoefficient ? -0.5 : 0.5;
            }

            var currentState = new AgentState(passenger.Neighbors, passenger.Satisfaction, passenger.TransportType).GetStringFormat();
            stateStorage.SaveStateReward(passenger.PreviousState, currentState, reward, passenger.TransportType);

            var allQualityCoefficients = passenger.AllQualityCoefficients;
            var averageQuality = allQualityCoefficients.Count > 0
                ? allQualityCoefficients.Skip(Math.Max(0, allQualityCoefficients.Count - 5)).Average()
                : 0;

            return (curretnQualityCoefficient - averageQuality + 1)/2 + passenger.QualityCoefficient;
        }
    }
}