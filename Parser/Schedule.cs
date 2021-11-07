using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser
{
    public class Schedule
    {

        private readonly Dictionary<DayOfWeek, List<Lesson>> WeekSchedule = new Dictionary<DayOfWeek, List<Lesson>>();

        public void AddInSchedule(Lesson lesson, DayOfWeek day)
        {
            WeekSchedule[day].Add(lesson);
        }
    }
}
