using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Subject))
            {
                return "";
            }

            var result = new StringBuilder();
            result.Append($"{Start.ToString("HH:mm")}-{End.ToString("HH:mm")} {Subject}");
            var teacher = Teacher;
            if (!string.IsNullOrEmpty(teacher) && teacher.StartsWith("Преподаватель: "))
            {
                teacher = teacher.Substring(15);
            }

            if (!string.IsNullOrEmpty(Location) && !string.IsNullOrEmpty(teacher))
            {
                result.Append($" ({Location}, {teacher})");
            }
            else if (!string.IsNullOrEmpty(Location))
            {
                result.Append($" ({Location})");
            }
            else if (!string.IsNullOrEmpty(teacher))
            {
                result.Append($" ({teacher})");
            }

            return result.ToString();
        }
    }
}