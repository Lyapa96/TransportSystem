using System.Collections.Generic;
using System.Linq;

namespace WebTransportSystem.Models
{
    public static class TransportSystem
    {
        public static void ChangeQualityCoefficients(IReadOnlyList<Passenger> passengers)
        {
            var carCount = passengers.Count(x => x.TransportType == TransportType.Car);

            foreach (var passenger in passengers)
            {
                if (passenger.TransportType == TransportType.Car)
                {
                    passenger.QualityCoefficient = GetQualityCoefficientForCar(carCount, passengers.Count, passenger);
                }
                else
                {
                    passenger.QualityCoefficient = GetQualityCoefficientForBus(passenger);
                }
            }
        }

        private static double GetQualityCoefficientForBus(Passenger passenger)
        {
            if (passenger.Number <= 3)
            {
                return 0.3;
            }
            if (passenger.Number <= 6)
            {
                return 0.4;
            }

            return 0.5;
        }

        private static double GetQualityCoefficientForCar(int carCount, int n, Passenger passenger)
        {
            if (passenger.Neighbors.Count(x => x.TransportType == TransportType.Car) < 2)
            {
                var answer = 1 - (double)carCount / n;
                return answer;
            }

            return 0.1;
        }
    }
}