using System;

namespace RocketLandingAssistant.Model
{
    internal class LandingCheck
    {
        public RocketId RocketId { get; }
        public LandingPosition LandingPosition { get; }
        public DateTime Timestamp { get; }

        public LandingCheck(RocketId rocketId, LandingPosition landingPosition, DateTime timestamp)
        {
            RocketId = rocketId;
            LandingPosition = landingPosition;
            Timestamp = timestamp;
        }
    }
}
