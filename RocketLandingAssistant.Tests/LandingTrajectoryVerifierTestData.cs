using System.Collections.Generic;
using RocketLandingAssistant.Constants;

namespace RocketLandingAssistant.Tests
{
    public partial class LandingTrajectoryVerifierTests
    {
        public static IEnumerable<object[]> LatestLandingClashTestData = new[]
        {
            new object[] {5, 5, 10, 10, 10, 10, OutputMessages.Clash},
            new object[] {10, 10, 5, 5, 10, 10, OutputMessages.OkForLanding}
        };

        public static IEnumerable<object[]> ClashTestData = new[]
        {
            new object[] {1, 2, 10, 10, 10, 10, OutputMessages.Clash},
            new object[] {1, 2, 10, 10, 11, 10, OutputMessages.Clash},
            new object[] {1, 2, 10, 10, 10, 11, OutputMessages.Clash},
            new object[] {1, 1, 10, 10, 10, 11, OutputMessages.OkForLanding},
            new object[] {1, 2, 10, 10, 12, 10, OutputMessages.OkForLanding},
            new object[] {1, 2, 10, 10, 10, 12, OutputMessages.OkForLanding}
        };

        public static IEnumerable<object[]> LandingAreaTestData = new[]
        {
            new object[] {0, 0},
            new object[] {1, 0},
            new object[] {101, 101},
            new object[] {100, 101}
        };

        public static IEnumerable<object[]> PlatformTestData = new[]
        {
            new object[] {1, 1, OutputMessages.OutOfPlatform},
            new object[] {100, 100, OutputMessages.OutOfPlatform},
            new object[] {4, 5, OutputMessages.OutOfPlatform},
            new object[] {5, 4, OutputMessages.OutOfPlatform},
            new object[] {15, 16, OutputMessages.OutOfPlatform},
            new object[] {16, 15, OutputMessages.OutOfPlatform},
            new object[] {16, 5, OutputMessages.OutOfPlatform},
            new object[] {5, 16, OutputMessages.OutOfPlatform},
            new object[] {5, 5, OutputMessages.OkForLanding},
            new object[] {15, 15, OutputMessages.OkForLanding},
            new object[] {8, 8, OutputMessages.OkForLanding}
        };
    }
}