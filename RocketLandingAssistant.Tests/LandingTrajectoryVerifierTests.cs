using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RocketLandingAssistant.Constants;
using RocketLandingAssistant.Model;
using RocketLandingAssistant.Providers;
using Xunit;

namespace RocketLandingAssistant.Tests
{
    public partial class LandingTrajectoryVerifierTests
    {
        private readonly LandingTrajectoryVerifier _sut;

        public LandingTrajectoryVerifierTests()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            dateTimeProviderMock
                .SetupSequence(dateTimeProvider => dateTimeProvider.DateTimeUtcNow)
                .Returns(new DateTime(2020, 01, 01))
                .Returns(new DateTime(2020, 01, 02))
                .Returns(new DateTime(2020, 01, 03))
                .Returns(new DateTime(2020, 01, 04))
                .Returns(new DateTime(2020, 01, 05))
                .Returns(new DateTime(2020, 01, 06));

            _sut = LandingTrajectoryVerifier.Initialize(
                LandingPosition.From(new Position(5, 5)),
                LandingPosition.From(new Position(15, 15)),
                dateTimeProviderMock.Object);
        }

        [Theory]
        [MemberData(nameof(PlatformTestData))]
        public async Task VerifyPosition_ShouldDetectPlatform_WhenProvidedValidLandingPosition(int x, int y, string expectedOutput)
        {
            var result = await _sut.VerifyPosition(1, LandingPosition.From(new Position(x, y)));

            result.Should().Be(expectedOutput);
        }

        [Theory]
        [MemberData(nameof(LandingAreaTestData))]
        public void VerifyPosition_ShouldThrow_WhenProvidedOutOfLandingAreaLandingPosition(int x, int y)
        {
            Func<Task> action = async () => await _sut.VerifyPosition(1, LandingPosition.From(new Position(x, y)));

            action.Should().Throw<LandingPositionOutsideLandingAreaException>();
        }

        [Theory]
        [MemberData(nameof(ClashTestData))]
        public async Task VerifyPosition_ShouldDetectClash_WhenMultipleRocketsAreLanding(
            int rocket1Id, int rocket2Id, int x1, int y1, int x2, int y2, string expectedOutput)
        {
            await _sut.VerifyPosition(rocket1Id, LandingPosition.From(new Position(x1, y1)));
            var result = await _sut.VerifyPosition(rocket2Id, LandingPosition.From(new Position(x2, y2)));

            result.Should().Be(expectedOutput);
        }

        [Theory]
        [MemberData(nameof(LatestLandingClashTestData))]
        public async Task VerifyPosition_ShouldDetectClashBasedOnLatestLandingCheck_WhenMultipleRocketsAreLanding(
            int x1, int y1, int x2, int y2, int x3, int y3, string expectedOutput)
        {
            await _sut.VerifyPosition(1, LandingPosition.From(new Position(x1, y1)));
            await _sut.VerifyPosition(1, LandingPosition.From(new Position(x2, y2)));
            var result = await _sut.VerifyPosition(2, LandingPosition.From(new Position(x3, y3)));

            result.Should().Be(expectedOutput);
        }

        [Fact]
        public async Task VerifyPosition_ShouldProvideCorrectOutput_WhenMultipleRocketsAreLandingInParallel()
        {
            var (rocketId0, landingPosition0) = (1, LandingPosition.From(new Position(12, 12)));
            var (rocketId1, landingPosition1) = (1, LandingPosition.From(new Position(5, 5)));
            var (rocketId2, landingPosition2) = (2, LandingPosition.From(new Position(6, 6)));
            var (rocketId3, landingPosition3) = (3, LandingPosition.From(new Position(9, 9)));
            var (rocketId4, landingPosition4) = (4, LandingPosition.From(new Position(8, 8)));
            var (rocketId5, landingPosition5) = (5, LandingPosition.From(new Position(15, 15)));

            await Task.WhenAll(
                    RunLandingCheck(rocketId0, landingPosition0, OutputMessages.OkForLanding),
                    RunLandingCheck(rocketId1, landingPosition1, OutputMessages.OkForLanding),
                    RunLandingCheck(rocketId2, landingPosition2, OutputMessages.Clash),
                    RunLandingCheck(rocketId3, landingPosition3, OutputMessages.OkForLanding),
                    RunLandingCheck(rocketId4, landingPosition4, OutputMessages.Clash),
                    RunLandingCheck(rocketId5, landingPosition5, OutputMessages.OkForLanding)
            );
        }

        private Task RunLandingCheck(int rocketId, LandingPosition landingPosition, string expectedOutput)
        {
            return _sut.VerifyPosition(rocketId, landingPosition)
                .ContinueWith(task => task.Result.Should().BeEquivalentTo(expectedOutput));
        }
    }
}
