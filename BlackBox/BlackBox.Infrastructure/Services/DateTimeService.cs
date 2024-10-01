using BlackBox.Application.Common.Interfaces;

namespace BlackBox.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime EstNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }
}
