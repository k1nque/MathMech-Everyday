using System;

namespace Parser
{
    public class Lesson
    {
        public string Location { get; }
        public string Teacher { get; }
        public string Subject { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public Lesson(string location, string teacher, string subject,
            DateTime start, DateTime end)
        {
            Location = location;
            Teacher = teacher;
            Subject = subject;
            Start = start;
            End = end;
        }
    }
}