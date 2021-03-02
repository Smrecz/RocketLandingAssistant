using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using RocketLandingAssistant;
using RocketLandingAssistant.Model;

namespace RocketLandingAssistantBenchmark
{
    public class Program
    {
        public class LandingBenchmark
        {
            private readonly LandingTrajectoryVerifier _landingTrajectoryVerifier = LandingTrajectoryVerifier.Initialize(
                LandingPosition.From(new Position(5, 5)),
                LandingPosition.From(new Position(15, 15)));

            [GlobalSetup]
            public async Task GlobalSetup()
            {
                var rocketId = RocketId.From(1);
                var landingPosition = LandingPosition.From(new Position(5, 5));

                await _landingTrajectoryVerifier.VerifyPosition(rocketId, landingPosition);
            }

            [Benchmark]
            public Task LandOnPlatform()
            {
                var rocketId = RocketId.From(1);
                var landingPosition = LandingPosition.From(new Position(5, 5));

                return _landingTrajectoryVerifier.VerifyPosition(rocketId, landingPosition);
            }

            [Benchmark]
            public Task LandOutsideOfPlatform()
            {
                var rocketId = RocketId.From(1);
                var landingPosition = LandingPosition.From(new Position(1, 1));

                return _landingTrajectoryVerifier.VerifyPosition(rocketId, landingPosition);
            }

            [Benchmark]
            public Task Clash()
            {
                var rocketId = RocketId.From(2);
                var landingPosition = LandingPosition.From(new Position(5, 5));

                return _landingTrajectoryVerifier.VerifyPosition(rocketId, landingPosition);
            }

            [GlobalCleanup]
            public void GlobalCleanup()
            {
                _landingTrajectoryVerifier.Dispose();
            }
        }

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<LandingBenchmark>();
        }
    }
}
