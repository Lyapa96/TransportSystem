﻿using System;
using System.Linq;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Models.SatisfactionDetermination
{
    public class QLearningQualityCoefficientAlgorithm : ISatisfactionDeterminationAlgorithm
    {
        private const double RewardWorth = 0.5;
        private readonly IAgentStateStorage stateStorage;

        public QLearningQualityCoefficientAlgorithm(IAgentStateStorage stateStorage)
        {
            this.stateStorage = stateStorage;
        }

        public double GetSatisfaction(Passenger passenger)
        {
            var currentQualityCoefficient = passenger.QualityCoefficient;
            var reward = 0.0;
            if (passenger.AllQualityCoefficients.Count != 0)
            {
                var previousQualityCoefficient = passenger.AllQualityCoefficients.Last();
                reward = GetReward(previousQualityCoefficient, currentQualityCoefficient);
            }

            var currentState = new AgentState(passenger.Neighbors, passenger.Satisfaction, passenger.TransportType).GetStringFormat();
            stateStorage.SaveStateReward(passenger.PreviousState, currentState, reward, passenger.TransportType);

            var allQualityCoefficients = passenger.AllQualityCoefficients;
            var averageQuality = allQualityCoefficients.Count > 0
                ? allQualityCoefficients.Skip(Math.Max(0, allQualityCoefficients.Count - 5)).Average()
                : 0;

            return (currentQualityCoefficient - averageQuality + 1)/2 + passenger.QualityCoefficient;
        }

        private double GetReward(double previousQualityCoefficient, double currentQualityCoefficient)
        {
            return previousQualityCoefficient > currentQualityCoefficient ? -RewardWorth : RewardWorth;
        }
    }
}