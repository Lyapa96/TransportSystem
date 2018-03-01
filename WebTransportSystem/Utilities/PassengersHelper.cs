using System;
using System.Linq;
using WebTransportSystem.Models;

namespace WebTransportSystem.Utilities
{
    public static class PassengersHelper
    {
        public static Passenger CreatePassenger(int number)
        {
            var rnd = new Random();
            var transport = rnd.Next(2) == 1 ? TransportType.Bus : TransportType.Car;
            var quality = Math.Round(rnd.NextDouble(), 2);
            var satisfaction = Math.Round(rnd.NextDouble(), 2);
            return new Passenger(transport, quality, satisfaction, number);
        }

        public static void SetNeighborsPassengers(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (i > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i - 1][j]);
                    }
                    if (j > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j - 1]);
                    }
                    if (i < rowCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i + 1][j]);
                    }
                    if (j < columnCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j + 1]);
                    }
                }
            }
        }
    }
}