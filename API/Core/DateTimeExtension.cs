using System;

namespace API.Core
{
    public static class DateTimeExtension
    {
        public static DateTime toEndOfDayUtc(this DateTime UtcTime, TimeZoneInfo timeZone)
        {
            UtcTime.checkUtc();
            return TimeZoneInfo.ConvertTimeFromUtc(UtcTime, timeZone).Date.AddDays(1).AddTicks(-1).ToUniversalTime();
        }

        public static DateTime toBeginOfDayUtc(this DateTime UtcTime, TimeZoneInfo timeZone)
        {
            UtcTime.checkUtc();
            return TimeZoneInfo.ConvertTimeFromUtc(UtcTime, timeZone).Date.ToUniversalTime();
        }

        public static void checkUtc(this DateTime time)
        {
            if (time.Kind != DateTimeKind.Utc)
            {
                throw new Exception("dateTime is not UTC");
            }
        }
    }
}