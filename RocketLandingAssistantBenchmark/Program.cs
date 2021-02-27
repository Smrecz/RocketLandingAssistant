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

            [Benchmark]
            public async Task LandOnPlatform() => await _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(5, 5)));

            [Benchmark]
            public async Task LandOutsideOfPlatform() => await _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(1, 1)));

            [Benchmark]
            public async Task Clash()
            {
                await Task.WhenAll(
                    _landingTrajectoryVerifier.VerifyPosition(1, LandingPosition.From(new Position(5, 5))),
                            _landingTrajectoryVerifier.VerifyPosition(2, LandingPosition.From(new Position(6, 6)))
                    );
            }
        }

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<LandingBenchmark>();
        }
    }
}
