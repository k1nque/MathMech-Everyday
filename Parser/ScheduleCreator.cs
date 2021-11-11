using System;
using System.Globalization;
using System.Linq;
using System.Net;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Calendar = Ical.Net.Calendar;

namespace Parser
{
    public static class ScheduleCreator
    {
        private static DateTime GetLastSundayDate(DateTime dateTime) =>
            dateTime.AddDays(-(int) dateTime.DayOfWeek);
        
        private static int GetWeekOfYear(CalendarEvent calEvent) =>
            CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                calEvent.DtStart.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

        private static DateTime ConvertIDateTimeToDateTime(IDateTime dt) =>
            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        
        private static void DownloadCalendar(int institute, int course, int groupId, DateTime dateTime)
        {
            var id = GroupIdFinder.FindGroupId(
                institute.ToString(),
                course.ToString(),
                groupId.ToString());
            var lastSundayDate = GetLastSundayDate(dateTime);
            var dt = lastSundayDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            using var client = new WebClient();
            client.DownloadFile("https://urfu.ru/api/schedule/groups/calendar/" +
                                $"{id.ToString()}/{dt}/", "calendar.ics");
        }

        private static Calendar LoadCalendar(string fileName)
        {
            var file = new System.IO.StreamReader(fileName);
            var content = file.ReadToEnd();
            file.Close();
            //TODO Delete calendar.ics
            return Calendar.Load(content);
        }

        private static Schedule ParseCalendar(ICalendarObject calendar)
        {
            var schedule = new Schedule();
            var calendarChildren = calendar.Children
                .Cast<CalendarEvent>().ToList();
            var firstEvent = calendarChildren.First();
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

        public static Schedule CreateSchedule(
            int groupId, int course, DateTime time, int institute = 25714)
        {
            DownloadCalendar(institute, course, groupId, time);
            var calendar = LoadCalendar("calendar.ics");
            return ParseCalendar(calendar);
        }
    }
}
