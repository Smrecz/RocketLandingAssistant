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
                await _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(5, 5)));
            }

            [Benchmark]
            public Task LandOnPlatform()
            {
                return _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(5, 5)));
            }

            [Benchmark]
            public Task LandOutsideOfPlatform()
            {
                return _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(1, 1)));
            }

            [Benchmark]
            public Task Clash()
            {
                return _landingTrajectoryVerifier.VerifyPosition(2, LandingPosition.From(new Position(5, 5)));
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
