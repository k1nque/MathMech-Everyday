using System;
using System.Globalization;
using System.Net;
using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;

namespace Parser
{
    public static class ScheduleCreator
    {
        private static DateTime GetLastSundayDate(DateTime dateTime) =>
            dateTime.AddDays(-(int) dateTime.DayOfWeek - 1);
        
        public static void DownloadCalendar(int groupId, int course, DateTime dateTime)
        {
            var id = GroupIdFinder.FindGroupId(groupId.ToString(), course.ToString());
            var lastSundayDate = GetLastSundayDate(dateTime);
            var dt = lastSundayDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            using var client = new WebClient();
            client.DownloadFile($"https://urfu.ru/api/schedule/groups/calendar/{id.ToString()}/{dt}/", "calendar.ics");
        }

        public static Calendar LoadCalendar(string fileName)
        {
            var file = new System.IO.StreamReader(fileName);
            var content = file.ReadToEnd();
            file.Close();
            //TODO Delete calendar.ics
            return Calendar.Load(content);
        }

        public static Schedule ParseCalendar(Calendar calendar)
        {
            var schedule = new Schedule();
            foreach (var item in calendar.Children)
            {
                if (item is not CalendarEvent calendarEvent) continue;
                var lesson = new Lesson(
                    calendarEvent.Location,
                    calendarEvent.Description,
                    calendarEvent.Summary,
                    calendarEvent.DtStart.Date,
                    calendarEvent.DtEnd.Date
                );
                schedule.AddLesson(lesson, calendarEvent.DtStart.DayOfWeek);
            }

            return schedule;
        }
    }
}
