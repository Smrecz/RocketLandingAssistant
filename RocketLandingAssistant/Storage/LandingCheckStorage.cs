using System;
using System.Collections.Generic;
using RocketLandingAssistant.Model;

namespace RocketLandingAssistant.Storage
{
    internal class LandingCheckStorage : ILandingCheckStorage
    {
        private readonly Dictionary<RocketId, LandingCheck> _landingChecks = new Dictionary<RocketId, LandingCheck>();

        public void AddLandingCheck(LandingCheck landingCheck)
        {
            _landingChecks[landingCheck.RocketId] = landingCheck;
        }

        public LandingPosition GetPotentialClashPosition(LandingCheck landingCheck)
        {
            var previousLandingCheck = GetPreviousLandingCheck(landingCheck.RocketId, landingCheck.Timestamp);

            return previousLandingCheck?.LandingPosition;
        }

        private LandingCheck GetPreviousLandingCheck(RocketId currentRocketId, DateTime currentTimestamp)
        {
            LandingCheck previousLandingCheck = null;

            foreach (var (rocketId, landingCheck) in _landingChecks)
            {
                if (IsSameRocket(currentRocketId, rocketId))
                {
                    continue;
                }

                if (IsAfterCurrentCheck(currentTimestamp, landingCheck))
                {
                    continue;
                }

                if (IsOlderThanAlreadyFound(previousLandingCheck, landingCheck))
                {
                    continue;
                }

                previousLandingCheck = landingCheck;
            }

            return previousLandingCheck;
        }

        private static bool IsOlderThanAlreadyFound(LandingCheck previousLandingCheck, LandingCheck landingCheck)
        {
            if (previousLandingCheck is null)
            {
                return false;
            }

            return landingCheck.Timestamp < previousLandingCheck.Timestamp;
        }

        private static bool IsAfterCurrentCheck(DateTime currentTimestamp, LandingCheck landingCheck)
        {
            return landingCheck.Timestamp >= currentTimestamp;
        }

        private static bool IsSameRocket(RocketId currentRocketId, RocketId rocketId)
        {
            return rocketId == currentRocketId;
        }
    }
}
