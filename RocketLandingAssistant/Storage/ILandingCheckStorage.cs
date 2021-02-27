using RocketLandingAssistant.Model;

namespace RocketLandingAssistant.Storage
{
    public interface ILandingCheckStorage
    {
        void AddLandingCheck(LandingCheck landingCheck);
        LandingPosition GetPotentialClashPosition(LandingCheck landingCheck);
    }
}