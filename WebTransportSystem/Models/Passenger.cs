using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Models
{
    public class Passenger
    {
        private const double PersonalSatisfaction = 0.1;
        private TransmissionFuncFactory factory;

        public Passenger(TransportType transportType,
            TransmissionType transmissionType,
            double qualityCoefficient,
            double satisfaction,
            int number)
        {
            TransmissionType = transmissionType;
            Number = number;
            TransportType = transportType;
            QualityCoefficient = qualityCoefficient;
            Satisfaction = satisfaction;
            Neighbors = new HashSet<Passenger>();
            factory = new TransmissionFuncFactory();
        }

        public Passenger()
        {
            Neighbors = new HashSet<Passenger>();
            AllQualityCoefficients = new List<double>();
            factory = new TransmissionFuncFactory();
        }

        public int Number { get; set; }
        public TransportType TransportType { get; set; }
        public double QualityCoefficient { get; set; }
        public double Satisfaction { get; set; }
        public HashSet<Passenger> Neighbors { get; set; }
        public List<double> AllQualityCoefficients { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public double DeviationValue { get; set; }

        private double GetAverageQuality => AllQualityCoefficients.Count > 0
            ? AllQualityCoefficients.Skip(Math.Max(0, AllQualityCoefficients.Count() - 5)).Average()
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
            TransportType = factory.GetTransmissionFunc(TransmissionType)
                .ChooseNextTransportType(Neighbors, TransportType, Satisfaction, DeviationValue);
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