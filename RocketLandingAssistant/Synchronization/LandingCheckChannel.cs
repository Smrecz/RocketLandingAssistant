using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RocketLandingAssistant.Synchronization
{
    internal class LandingCheckChannel : IDisposable
    {
        private readonly Channel<LandingCheckRequest> _landingCheckChannel = Channel.CreateUnbounded<LandingCheckRequest>();

        public Channel<LandingCheckRequest> GetChannel()
        {
            return _landingCheckChannel;
        }

        public ValueTask WriteAsync(LandingCheckRequest landingCheckRequest)
        {
            return _landingCheckChannel.Writer.WriteAsync(landingCheckRequest);
        }

        public void Dispose()
        {
            _landingCheckChannel.Writer.TryComplete();
        }
    }
}
