using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace Parser
{
    public class DaySchedule
    {
        public DayOfWeek Day { get; }
        public List<Lesson> Schedule { get; } = new();
        public DaySchedule(DayOfWeek day) => Day = day;

        public override string ToString()
        {
            var dayName = new CultureInfo("ru-RU").DateTimeFormat.GetDayName(Day);
            var joiningList = new List<string>
            {
                $"{CultureInfo.InvariantCulture.TextInfo.ToTitleCase(dayName)}:"
            };
            joiningList.AddRange(Schedule
                .Select(lesson => lesson.ToString()));
            return string.Join("\n", joiningList);
        }
    }

    public class Schedule
    {
        private readonly List<DaySchedule> weekSchedule = new();
        public IEnumerable<DaySchedule> WeekSchedule => weekSchedule;

        public Schedule()
        {
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            foreach (var day in daysOfWeek)
                weekSchedule.Add(new DaySchedule(day));
        }

        public void AddLesson(Lesson lesson, DayOfWeek day)
        {
            var daySchedule = weekSchedule.First(dS => dS.Day == day);
            daySchedule.Schedule.Add(lesson);
        }

        public DaySchedule GetDaySchedule(DayOfWeek day) =>
            weekSchedule.First(dS => dS.Day == day);

        public override string ToString()
        {
            var joiningList = weekSchedule
                .OrderBy(daySchedule => ((int) daySchedule.Day + 6) % 7) // monday first
                .Take(6) // skip sunday
                .Select(daySchedule => daySchedule.ToString());
            return string.Join("\n", joiningList);
        }
    }
}