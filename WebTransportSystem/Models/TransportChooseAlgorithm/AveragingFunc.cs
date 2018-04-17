using System;
using System.Collections.Generic;
using System.Linq;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class AveragingFunc : ITransmissionFunc
    {
        public TransportType ChooseNextTransportType(HashSet<Passenger> neighbors, TransportType currentTransportType, double currentSatisfaction)
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
                    currentTransportType = info.Item1;

            if (currentTransportType == TransportType.Car)
            {
                var rnd = new Random();
                currentTransportType = rnd.Next(1, 100) < 85 ? TransportType.Car : TransportType.Bus;
            }

            return currentTransportType;
        }
    }
}