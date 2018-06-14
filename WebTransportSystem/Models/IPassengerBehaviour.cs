using WebTransportSystem.Models.SatisfactionDetermination;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Models
{
    public interface IPassengerBehaviour
    {
        ITransmissionFunc GetTransmissionFunc(TransmissionType transmissionType);
        ISatisfactionDeterminationAlgorithm GetSatisfactionDeterminationAlgorithm(TransmissionType transmissionType);
    }
}