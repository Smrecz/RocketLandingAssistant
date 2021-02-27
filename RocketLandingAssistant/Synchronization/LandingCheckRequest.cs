using System.Threading.Tasks;
using RocketLandingAssistant.Model;

namespace RocketLandingAssistant.Synchronization
{
    public class LandingCheckRequest
    {
        private readonly TaskCompletionSource<LandingPosition> _taskCompletionSource;

        public LandingCheck LandingCheck { get; }

        public LandingCheckRequest(LandingCheck landingCheck)
        {
            LandingCheck = landingCheck;
            _taskCompletionSource = new TaskCompletionSource<LandingPosition>(TaskCreationOptions.None);
        }

        public void SetResult(LandingPosition landingPosition)
        {
           _taskCompletionSource.SetResult(landingPosition);
        }

        public Task<LandingPosition> GetResultAsync()
        {
            return _taskCompletionSource.Task;
        }
    }
}
