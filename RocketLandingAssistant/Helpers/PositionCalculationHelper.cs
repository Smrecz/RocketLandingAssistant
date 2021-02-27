using System.Collections.Generic;
using RocketLandingAssistant.Model;

namespace RocketLandingAssistant.Helpers
{
    internal class PositionCalculationHelper
    {
        public static HashSet<LandingPosition> GetPlatformPositionsFromCorners(LandingPosition leftTopPosition,
            LandingPosition rightBottomPosition)
        {
            var landingPlatformPositions = new HashSet<LandingPosition>();

            for (var x = leftTopPosition.Value.X; x <= rightBottomPosition.Value.X; x++)
            {
                for (var y = leftTopPosition.Value.Y; y <= rightBottomPosition.Value.Y; y++)
                {
                    landingPlatformPositions.Add(LandingPosition.From(new Position(x, y)));
                }
            }

            return landingPlatformPositions;
        }
    }
}
