using System;
using WebTransportSystem.Models;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Tests
{
    public class TestHelpers
    {
        public static Passenger CreatePassenger(
            IPassengerBehaviour passengerBehaviour,
            int number,
            TransmissionType transmissionType,
            TransportType transport,
            double satisfaction)
        {
            var rnd = new Random();
            var quality = Math.Round(rnd.NextDouble(), 2);
            return new Passenger(passengerBehaviour, transport, transmissionType, quality, satisfaction, number);
        }
    }
}