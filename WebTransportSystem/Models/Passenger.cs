using System;
using System.Collections.Generic;
using System.Linq;

namespace WebTransportSystem.Models
{
    public class Passenger
    {
        private const double PersonalSatisfaction = 0.1;

        public Passenger(TransportType transportType,
            double qualityCoefficient,
            double satisfaction,
            int number)
        {
            Number = number;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
        }

        public Passenger()
        {
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = new List<double>();
        }

        public int Number { get; set; }
        public TransportType TransportType { get; set; }
        public double QualityCoefficient { get; set; }
        public double Satisfaction { get; set; }
        public HashSet<Passenger> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }

        private double GetAverageQuality => AllQualityCoefficients.Count > 0 ?
            AllQualityCoefficients.Skip(Math.Max(0, AllQualityCoefficients.Count() - 5)).Average()
            : 0;

        public void AddNeighbors(params Passenger[] neighbors)
        {
            foreach (var passenger in neighbors)
            {
                Neighbors.Add(passenger);

                passenger.Neighbors.Add(this);
            }
        }

        public void AddNeighbor(Passenger neighbor)
        {
            Neighbors.Add(neighbor);
        }

        public void ChooseNextTransportType()
        {
            var typeTransportInfos = Neighbors
                .GroupBy(x => x.TransportType)
                .Select(type =>
                {
                    var averageSatisfaction = type.Select(x => x.Satisfaction).Average();
                    return Tuple.Create(type.Key, averageSatisfaction);
                });

            foreach (var info in typeTransportInfos)
                if (info.Item2 > Satisfaction)
                    TransportType = info.Item1;

            if (TransportType == TransportType.Car)
            {
                var rnd = new Random();
                TransportType = rnd.Next(1, 100) < 85 ? TransportType.Car : TransportType.Bus;
            }
        }

        public void UpdateSatisfaction()
        {
            Satisfaction = (QualityCoefficient - GetAverageQuality + 1) / 2 + PersonalSatisfaction;
            AllQualityCoefficients.Add(QualityCoefficient);
        }

        public override string ToString()
        {
            //var allNeighbors = Neighbors.Select(x => x.Number.ToString()).Aggregate((x, y) => x + "," + y);
            return $"{TransportType} k=({QualityCoefficient:0.00}) S=({Satisfaction:0.00})";
        }
    }
}