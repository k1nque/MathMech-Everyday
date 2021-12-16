using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ical.Net;
using Ical.Net.CalendarComponents;
using static System.Globalization.CultureInfo;

namespace Parser
{
    public class ScheduleCreator : IScheduleCreator
    {
        private readonly IGroupIdFinder groupIdFinder;
        public ScheduleCreator(IGroupIdFinder finder) => groupIdFinder = finder;

        private int GetWeekOfYear(CalendarEvent calEvent) =>
            InvariantCulture.Calendar.GetWeekOfYear(
                calEvent.DtStart.Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

        private Schedule CreateSchedule(List<CalendarEvent> calendarEvents, int firstEventWeek)
        {
            var schedule = new Schedule();
            foreach (var calendarEvent in calendarEvents)
            {
                var currentEventWeek = GetWeekOfYear(calendarEvent);
                if (firstEventWeek != currentEventWeek)
                    break;
                var start = calendarEvent.DtStart.ToDateTime();
                var end = calendarEvent.DtEnd.ToDateTime();
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

        private Schedule ParseCalendar(ICalendarObject calendar)
        {
            var schedule = new Schedule();
            var calendarChildren = calendar.Children
                .Cast<CalendarEvent>().ToList();
            var firstEvent = calendarChildren.FirstOrDefault();
            if (firstEvent == null)
                return schedule;
            var firstEventWeek = GetWeekOfYear(firstEvent);

            return CreateSchedule(calendarChildren, firstEventWeek);
        }

        public async Task<Schedule> CreateScheduleById(string groupId, DateTime time)
        {
            var calendar = await new CalendarCreator().GetCalendar(groupId, time);
            return ParseCalendar(calendar);
        }

        public async Task<Schedule> CreateScheduleByName(string groupName, DateTime time)
        {
            var id = groupIdFinder.FindGroupId(groupName);
            return await CreateScheduleById(id, time);
        }
    }
}