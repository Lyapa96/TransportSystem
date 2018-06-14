using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using WebTransportSystem.Models;
using WebTransportSystem.Models.SatisfactionDetermination;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Tests
{
    public class SatisfactionAlgorithmSpec
    {
        private LastFiveTripsAlgorithm satisfactionAlgorithm;
        private IPassengerBehaviour passengerBehaviour;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviour>();
            satisfactionAlgorithm = new LastFiveTripsAlgorithm();
        }

        [Test]
        public void Should_return_rigth_satisfation()
        {
            var passenger = TestHelpers.CreatePassenger(passengerBehaviour, 1, TransmissionType.Average, TransportType.Bus, 0.1);
            passenger.QualityCoefficient = 1;

            passenger.AllQualityCoefficients = new List<double>(){1,1,1,1,1};

            var result = satisfactionAlgorithm.GetSatisfaction(passenger);

            result.Should().Be(1.5);
        }
    }
}