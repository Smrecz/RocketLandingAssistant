using System;

namespace RocketLandingAssistant.Providers
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTimeUtcNow => DateTime.UtcNow;
    }
}
