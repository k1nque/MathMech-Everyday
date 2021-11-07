using System;

namespace Parser
{
    public class Lesson
    {
        public string Location { get; private set; }
        public string Teacher { get; private set; }
        public string Subject { get; private set; }

        public Lesson(string location, string teacher, string subject)
        {
            Location = location;
            Teacher = teacher;
            Subject = subject;
        }
    }
}
