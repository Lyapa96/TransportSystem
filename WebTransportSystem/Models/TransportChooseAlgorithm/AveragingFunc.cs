﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class AveragingFunc : ITransmissionFunc
    {
        private readonly double carAvailabilityProbability;
        private readonly Random rnd;

        public AveragingFunc(double carAvailabilityProbability)
        {
            rnd = new Random();
            this.carAvailabilityProbability = carAvailabilityProbability;
        }

        public TransportType ChooseNextTransportType(HashSet<Passenger> neighbors, TransportType currentTransportType, double currentSatisfaction, double deviationValue)
        {
            var typeTransportInfos = neighbors
                .GroupBy(x => x.TransportType)
                .Select(type =>
                {
                    var averageSatisfaction = type.Select(x => x.Satisfaction).Average();
                    return Tuple.Create(type.Key, averageSatisfaction);
                });

            foreach (var info in typeTransportInfos)
                if (info.Item2 > currentSatisfaction)
                {
                    currentTransportType = info.Item1;
                    currentSatisfaction = info.Item2;
                }
                    

            if (currentTransportType == TransportType.Car)
            {
                currentTransportType = rnd.NextDouble() < carAvailabilityProbability ? TransportType.Car : TransportType.Bus;
            }

            return currentTransportType;
        }
    }
}