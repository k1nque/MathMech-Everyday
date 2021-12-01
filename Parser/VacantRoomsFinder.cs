using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class VacantRoomsFinder
    {
        private static IEnumerable<DaySchedule> GetGroupsSchedule(
            IEnumerable<string> groupsId, DateTime dateTime) =>
            groupsId.Select(id => ScheduleCreator.CreateScheduleById(id, dateTime)
                .GetDaySchedule(dateTime.DayOfWeek));

        private static IEnumerable<Lesson> GetLessonsAtTime(
            IEnumerable<DaySchedule> groupsDaySchedule, DateTime dateTime) =>
            groupsDaySchedule
                .SelectMany(schedule => schedule.Schedule
                    .Where(lesson => lesson.Start <= dateTime && dateTime <= lesson.End));

        private static IEnumerable<string> GetLessonsRooms(IEnumerable<Lesson> lessons) =>
            lessons.Select(lesson => lesson.Location);

        public IEnumerable<string> FindVacant(DateTime dateTime) //institute?
        {
            var ids = new List<string>();
            var groupsSchedule = GetGroupsSchedule(ids, dateTime);
            var lessons = GetLessonsAtTime(groupsSchedule, dateTime);
            return GetLessonsRooms(lessons);
        }
    }
}