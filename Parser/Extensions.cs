using System;
using Ical.Net.DataTypes;

namespace Parser
{
    public static class Extensions
    {
        public static DateTime GetLastSunday(this DateTime dateTime) =>
            dateTime.AddDays(-(int) dateTime.DayOfWeek);
        
        public static DateTime ToDateTime(this IDateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }
}