using System;
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
    }
}