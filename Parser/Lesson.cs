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
                return default;

            var result = new StringBuilder();
            result.Append($"{Start:HH:mm}-{End:HH:mm} {Subject}");
            var teacher = Teacher;
            var isTeacherPresented = !string.IsNullOrEmpty(teacher);
            var isLocationPresented = !string.IsNullOrEmpty(Location);
            if (isTeacherPresented && teacher.StartsWith("Преподаватель: "))
                teacher = teacher.Substring(15);

            return isLocationPresented switch
            {
                true when isTeacherPresented =>
                    result.Append($" ({Location}, {teacher})").ToString(),
                true =>
                    result.Append($" ({Location})").ToString(),
                false when isTeacherPresented =>
                    result.Append($" ({teacher})").ToString(),
                _ => result.ToString()
            };
        }
    }
}