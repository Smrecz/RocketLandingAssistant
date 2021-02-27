using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RocketLandingAssistant.Constants;
using RocketLandingAssistant.Helpers;
using RocketLandingAssistant.Model;
using RocketLandingAssistant.Providers;
using RocketLandingAssistant.Storage;
using RocketLandingAssistant.Synchronization;

namespace RocketLandingAssistant
{
    public class LandingTrajectoryVerifier : IDisposable
    {
        private const int ClashDistance = 1;

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly HashSet<LandingPosition> _landingPlatformPositions;
        private readonly LandingCheckChannel _landingCheckChannel;

        private LandingTrajectoryVerifier(
            IDateTimeProvider dateTimeProvider,
            HashSet<LandingPosition> landingPlatformPositions, 
            LandingCheckConsumer landingCheckConsumer, 
            LandingCheckChannel landingCheckChannel)
        {
            _dateTimeProvider = dateTimeProvider;
            _landingPlatformPositions = landingPlatformPositions;
            _landingCheckChannel = landingCheckChannel;

            landingCheckConsumer?.Consume(_landingCheckChannel.GetChannel());
        }

        /// <summary>
        /// Initializes LandingTrajectoryVerifier instance with default IDateTimeProvider implementation.
        /// </summary>
        /// <param name="leftTopPosition">Top left corner of landing area rectangle.</param>
        /// <param name="rightBottomPosition">Bottom right corner of landing area rectangle.</param>
        /// <returns></returns>
        public static LandingTrajectoryVerifier Initialize(LandingPosition leftTopPosition, LandingPosition rightBottomPosition)
        {
            return Initialize(leftTopPosition, rightBottomPosition, new DateTimeProvider());
        }

        /// <summary>
        /// Initializes LandingTrajectoryVerifier instance.
        /// </summary>
        /// <param name="leftTopPosition">Top left corner of landing area rectangle.</param>
        /// <param name="rightBottomPosition">Bottom right corner of landing area rectangle.</param>
        /// <param name="dateTimeProvider">IDateTimeProvider implementation to be used.</param>
        /// <returns></returns>
        public static LandingTrajectoryVerifier Initialize(LandingPosition leftTopPosition, LandingPosition rightBottomPosition, IDateTimeProvider dateTimeProvider)
        {
            var landingPlatformPositions =
                PositionCalculationHelper.GetPlatformPositionsFromCorners(leftTopPosition, rightBottomPosition);

            return new LandingTrajectoryVerifier(dateTimeProvider, landingPlatformPositions, new LandingCheckConsumer(new LandingCheckStorage()), new LandingCheckChannel());
        }

        /// <summary>
        /// Verifies if given landing position is eligible for landing.
        /// </summary>
        /// <param name="rocketId">Rocket identifier.</param>
        /// <param name="position">Potential landing position to check.</param>
        /// <returns></returns>
        public async Task<string> VerifyPosition(int rocketId, LandingPosition position)
        {
            if (IsOutOfPlatform(position))
            {
                return OutputMessages.OutOfPlatform;
            }

            var landingCheck = new LandingCheck(rocketId, position, _dateTimeProvider.DateTimeUtcNow);

            var landingCheckRequest = new LandingCheckRequest(landingCheck);

            await _landingCheckChannel.WriteAsync(landingCheckRequest);

            var potentialClashPosition = await landingCheckRequest.GetResultAsync();

            return IsClashWithPrevious(position, potentialClashPosition)
                ? OutputMessages.Clash
                : OutputMessages.OkForLanding;
        }

        private static bool IsClashWithPrevious(LandingPosition position, LandingPosition potentialClashPosition)
        {
            if (potentialClashPosition is null)
            {
                return false;
            }

            var distanceX = potentialClashPosition.Value.X - position.Value.X;
            var distanceY = potentialClashPosition.Value.Y - position.Value.Y;

            return ClashX(distanceX) && ClashY(distanceY);
        }

        private static bool ClashY(int distanceY)
        {
            return distanceY <= ClashDistance 
                   && distanceY >= -ClashDistance;
        }

        private static bool ClashX(int distanceX)
        {
            return distanceX <= ClashDistance 
                   && distanceX >= -ClashDistance;
        }

        private bool IsOutOfPlatform(LandingPosition position)
        {
            return _landingPlatformPositions.Contains(position) == false;
        }

        public void Dispose()
        {
            _landingCheckChannel?.Dispose();
        }
    }
}
