using System.Threading.Channels;
using System.Threading.Tasks;
using RocketLandingAssistant.Storage;

namespace RocketLandingAssistant.Synchronization
{
    internal class LandingCheckConsumer
    {
        private readonly ILandingCheckStorage _landingCheckStorage;

        public LandingCheckConsumer(ILandingCheckStorage landingCheckStorage)
        {
            _landingCheckStorage = landingCheckStorage;
        }

        public async Task Consume(ChannelReader<LandingCheckRequest> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var landingCheckRequest))
                {
                    var potentialClashPosition = _landingCheckStorage.GetPotentialClashPosition(landingCheckRequest.LandingCheck);

                    _landingCheckStorage.AddLandingCheck(landingCheckRequest.LandingCheck);

                    landingCheckRequest.SetResult(potentialClashPosition);
                }
            }
        }
    }
}
