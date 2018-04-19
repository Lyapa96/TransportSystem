using System.Collections.Generic;

namespace WebTransportSystem.Models.TransportChooseAlgorithm
{
    public interface ITransmissionFunc
    {
        TransportType ChooseNextTransportType(HashSet<Passenger> neighbors, TransportType currentTransportType, double currentSatisfaction, double deviationValue);
    }
}