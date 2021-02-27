using System;

namespace RocketLandingAssistant.Providers
{
    public interface IDateTimeProvider
    {
        DateTime DateTimeUtcNow { get; }
    }
}