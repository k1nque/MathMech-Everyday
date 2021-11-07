using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class DaySchedule
    { 
        public enum Days
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        public readonly List<Lesson> Schedule;
        public readonly Days Day;

        public DaySchedule(List<Lesson> schedule, Days day)
        {
            Schedule = schedule;
            Day = day;
        }
    }
}
