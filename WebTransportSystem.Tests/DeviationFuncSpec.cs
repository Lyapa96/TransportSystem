﻿using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using WebTransportSystem.Models;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Tests
{
    public class DeviationFuncSpec
    {
        private double Deviation = 0.01;
        private DeviationFunc averagingFunc;
        private IPassengerBehaviour passengerBehaviour;
        private TransmissionType transmissionType;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviour>();
            transmissionType = TransmissionType.Deviation;
            averagingFunc = new DeviationFunc();
        }

        [Test]
        public void Should_return_ohter_transport_when_neighbors_has_better_satisfaction_and_deviation_enough()
        {
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };
            const TransportType expectedTransportType = TransportType.Bus;

            var transportType = averagingFunc.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, Deviation);

            transportType.Should().Be(expectedTransportType);
        }

        [Test]
        public void Should_return_same_transport_when_neighbors_has_better_satisfaction_but_deviation_not_enough()
        {
            Deviation = 0.5;
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };
            const TransportType expectedTransportType = TransportType.Car;

            var transportType = averagingFunc.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, Deviation);

            transportType.Should().Be(expectedTransportType);
        }
    }
}