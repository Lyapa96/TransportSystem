using System.Collections.Generic;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public class DeviationFunc : ITransmissionFunc
    {
        public TransportType ChooseNextTransportType(HashSet<Passenger> neighbors, TransportType currentTransportType,
            double currentSatisfaction)
        {
            return TransportType.Car;
        }
    }
}