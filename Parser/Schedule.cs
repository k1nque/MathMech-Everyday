using System;
using System.Collections.Generic;
using System.Linq;


namespace Parser
{
    public class DaySchedule
    {
        public DayOfWeek Day { get; }
        public List<Lesson> Schedule { get; } // TODO encapsulate
        
        public DaySchedule(DayOfWeek day)
        {
            Day = day;
            Schedule = new List<Lesson>();
        }
    }
    
    public class Schedule
    {
        public List<DaySchedule> WeekSchedule { get; } // TODO encapsulate

        public Schedule()
        {
            WeekSchedule = new List<DaySchedule>();
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            foreach (var day in daysOfWeek)
                WeekSchedule.Add(new DaySchedule(day));
        }
        
        public void AddLesson(Lesson lesson, DayOfWeek day)
        {
            var daySchedule = WeekSchedule.First(dS => dS.Day == day);
            daySchedule.Schedule.Add(lesson);
        }

        public DaySchedule GetDaySchedule(DayOfWeek day) =>
            WeekSchedule.First(dS => dS.Day == day);
    }
}
