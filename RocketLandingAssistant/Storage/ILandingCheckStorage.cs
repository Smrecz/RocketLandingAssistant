using RocketLandingAssistant.Model;

namespace RocketLandingAssistant.Storage
{
    internal interface ILandingCheckStorage
    {
        void AddLandingCheck(LandingCheck landingCheck);
        LandingPosition GetPotentialClashPosition(LandingCheck landingCheck);
    }
}