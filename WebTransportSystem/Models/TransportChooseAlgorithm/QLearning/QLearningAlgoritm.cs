﻿using System;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage.Sql;
using WebTransportSystem.Utilities;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning
{
    public static class QLearningAlgoritm
    {
        private const double Lf = 1;
        private const double Df = 0.1;
        private const double Epsilon = 0.15;
        private static readonly Random rnd = new Random();

        public static double GetUpdateReward(double previousReward, double maxNextReward, double currentReward)
        {
            return previousReward + Lf*(currentReward + Df*maxNextReward + previousReward);
        }

        public static TransportType GetBestNextTransportWithEpsilonMush(QFuncInfo qFuncInfo)
        {
            var bestTransportType = qFuncInfo.BusReward > qFuncInfo.CarReward ? TransportType.Bus : TransportType.Car;
            if (rnd.NextDouble() > Epsilon)
                return bestTransportType;
            return PassengersHelper.GetOtherRandomTransportType(bestTransportType);
        }
    }
}