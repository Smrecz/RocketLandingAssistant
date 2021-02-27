using System;

namespace RocketLandingAssistant.Model
{
    public class LandingCheck
    {
        public int RocketId { get; }
        public LandingPosition LandingPosition { get; }
        public DateTime Timestamp { get; }

        public LandingCheck(int rocketId, LandingPosition landingPosition, DateTime timestamp)
        {
            RocketId = rocketId;
            LandingPosition = landingPosition;
            Timestamp = timestamp;
        }
    }
}
