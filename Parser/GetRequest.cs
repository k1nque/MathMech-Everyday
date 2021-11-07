using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;

namespace Parser
{
    public static class GetRequest
    {
        public static void DownloadCalendar(int groupId, int course)
        {
            var id = DetectGroupId.FindId(groupId.ToString(), course.ToString());
            using (var client = new WebClient())
            {
                client.DownloadFile($"https://urfu.ru/api/schedule/groups/calendar/{id.ToString()}/20211107/", "calendar.ics");
            }
        }

        public static Calendar LoadCalendar(string fileName)
        {
            var file = new System.IO.StreamReader(fileName);
            string content = file.ReadToEnd();
            file.Close();
            //TODO Delete calendar.ics
            return Calendar.Load(content);
        }

        public static System.DayOfWeek ParseDate(string date)
        {
            var dt = DateTime.ParseExact(date, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
            return dt.DayOfWeek;
        }

        public static Schedule ParseCalendar(Calendar calendar)
        {
            Schedule schedule = new Schedule();
            foreach (ICalendarObject item in calendar.Children)
            {
                if (item is CalendarEvent calendarEvent)
                {
                    System.DayOfWeek dayOfWeek = ParseDate(calendarEvent.DtStart.ToString());
                    Lesson lesson = new Lesson(
                        calendarEvent.Location,
                        calendarEvent.Description,
                        calendarEvent.Summary);
                    schedule.AddInSchedule(lesson, dayOfWeek);

                }
            }

            return schedule;
        }
    }
}
