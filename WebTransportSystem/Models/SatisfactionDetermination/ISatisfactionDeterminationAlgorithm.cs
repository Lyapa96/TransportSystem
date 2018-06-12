namespace WebTransportSystem.Models.SatisfactionDetermination
{
    public interface ISatisfactionDeterminationAlgorithm
    {
        double GetSatisfaction(Passenger passenger);
    }
}