using System;
using ValueOf;

namespace RocketLandingAssistant.Model
{
    public class LandingPosition : ValueOf<Position, LandingPosition>
    {
        private const int LandingAreaMinimalPosition = 1;
        private const int LandingAreaMaximalPosition = 100;

        protected override void Validate()
        {
            if (IsBelowMinimum(Value))
            {
                throw new LandingPositionOutsideLandingAreaException(Value);
            }

            if (IsOverMaximum(Value))
            {
                throw new LandingPositionOutsideLandingAreaException(Value);
            }
        }

        private static bool IsBelowMinimum(Position position)
        {
            return position.X < LandingAreaMinimalPosition || position.Y < LandingAreaMinimalPosition;
        }

        private static bool IsOverMaximum(Position position)
        {
            return position.X > LandingAreaMaximalPosition || position.Y > LandingAreaMaximalPosition;
        }
    }

    public class LandingPositionOutsideLandingAreaException : Exception
    {
        public LandingPositionOutsideLandingAreaException(Position position) 
            : base($"Landing position: ({position.X}, {position.Y}) is outside of Landing Area.")
        {
            
        }
    }
}
