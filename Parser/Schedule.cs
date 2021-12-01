using System;
using System.Collections.Generic;
using System.Linq;


namespace Parser
{
    public class DaySchedule
    {
        public DayOfWeek Day { get; }
        public List<Lesson> Schedule { get; } = new();

        public DaySchedule(DayOfWeek day) => Day = day;
    }
    
    public class Schedule
    {
        private readonly List<DaySchedule> weekSchedule = new();

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
    }
}
