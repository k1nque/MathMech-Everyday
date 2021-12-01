using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using static System.Globalization.CultureInfo;
using Calendar = Ical.Net.Calendar;

namespace Parser
{
    public static class ScheduleCreator
    {
        private static DateTime GetLastSundayDate(DateTime dateTime) =>
            dateTime.AddDays(-(int) dateTime.DayOfWeek);

        private static int GetWeekOfYear(CalendarEvent calEvent) =>
            InvariantCulture.Calendar.GetWeekOfYear(
                calEvent.DtStart.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

        private static DateTime ConvertIDateTimeToDateTime(IDateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);

        private static void DownloadCalendar(string id, DateTime dateTime)
        {
            var lastSundayDate = GetLastSundayDate(dateTime);
            var dt = lastSundayDate.ToString("yyyyMMdd", InvariantCulture);
            using var client = new WebClient();
            client.DownloadFile("https://urfu.ru/api/schedule/groups/calendar/" +
                                $"{id}/{dt}/", "calendar.ics");
        }

        private static Calendar LoadCalendar(string fileName)
        {
            var file = new StreamReader(fileName);
            var content = file.ReadToEnd();
            file.Close();
            File.Delete(fileName);
            return Calendar.Load(content);
        }

        private static Schedule ParseCalendar(ICalendarObject calendar)
        {
            var schedule = new Schedule();
            var calendarChildren = calendar.Children
                .Cast<CalendarEvent>().ToList();
            var firstEvent = calendarChildren.FirstOrDefault();
            if (firstEvent == null)
                return schedule;
            var firstEventWeek = GetWeekOfYear(firstEvent);

            foreach (var calendarEvent in calendarChildren)
            {
                var currentEventWeek = GetWeekOfYear(calendarEvent);
                if (firstEventWeek != currentEventWeek)
                    break;
                var start = ConvertIDateTimeToDateTime(calendarEvent.DtStart);
                var end = ConvertIDateTimeToDateTime(calendarEvent.DtEnd);
                var lesson = new Lesson(
                    calendarEvent.Location,
                    calendarEvent.Description,
                    calendarEvent.Summary,
                    start,
                    end
                );
                schedule.AddLesson(lesson, calendarEvent.DtStart.DayOfWeek);
            }
            
            return schedule;
        }

        public static Schedule CreateScheduleById(string groupId, DateTime time)
        {
            DownloadCalendar(groupId, time);
            var calendar = LoadCalendar("calendar.ics");
            return ParseCalendar(calendar);
        }

        public static Schedule CreateScheduleByName(string groupName, DateTime time)
        {
            var id = GroupIdFinder.FindGroupId(groupName);
            return CreateScheduleById(id, time);
        }
    }
}