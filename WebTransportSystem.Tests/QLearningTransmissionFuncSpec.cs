using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using WebTransportSystem.Models;
using WebTransportSystem.Models.TransportChooseAlgorithm;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning;
using WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage;

namespace WebTransportSystem.Tests
{
    public class QLearningTransmissionFuncSpec
    {
        private const double deviationStub = 0;
        private IAgentStateStorage storage;
        private QLearningTransmissionFunc qLearningTransmissionFunc;
        private IPassengerBehaviour passengerBehaviour;

        [SetUp]
        public void SetUp()
        {
            passengerBehaviour = Substitute.For<IPassengerBehaviour>();
            storage = Substitute.For<IAgentStateStorage>();
            qLearningTransmissionFunc = new QLearningTransmissionFunc(storage);
        }

        [Test]
        public void Should_get_best_transport_from_storage()
        {
            const double currentSatisfaction = 0.4;
            var currentTransportType = TransportType.Car;
            var neighborsSatisfaction = currentSatisfaction + 0.1;
            var transmissionType = TransmissionType.QLearning;
            var neighbors = new HashSet<Passenger>
            {
                TestHelpers.CreatePassenger(passengerBehaviour, 1, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 2, transmissionType, TransportType.Car, neighborsSatisfaction),
                TestHelpers.CreatePassenger(passengerBehaviour, 3, transmissionType, TransportType.Car, neighborsSatisfaction)
            };

            qLearningTransmissionFunc.ChooseNextTransportType(neighbors, currentTransportType, currentSatisfaction, deviationStub);
            
            storage.Received().GetBestNextTransport(Arg.Any<string>());
        }
    }
}