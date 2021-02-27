using System;

namespace RocketLandingAssistant.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime DateTimeUtcNow => DateTime.UtcNow;
    }
}
