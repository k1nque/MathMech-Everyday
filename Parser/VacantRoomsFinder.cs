using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public static class VacantRoomsFinder
    {
        private static IEnumerable<DaySchedule> GetGroupsDaySchedules(
            IEnumerable<string> groupsId, DateTime dateTime) =>
            groupsId.Select(id => ScheduleCreator.CreateScheduleById(id, dateTime)
                .GetDaySchedule(dateTime.DayOfWeek));

        private static IEnumerable<Lesson> GetLessonsAtTime(
            IEnumerable<DaySchedule> groupsDaySchedule, DateTime dateTime) =>
            groupsDaySchedule
                .SelectMany(daySchedule => daySchedule.Schedule
                    .Where(lesson => lesson.Location != null)
                    .Where(lesson => lesson.Start <= dateTime && dateTime <= lesson.End));
        
        private static IEnumerable<string> GetLessonsRooms(IEnumerable<Lesson> lessons) =>
            lessons.Select(lesson => lesson.Location).Distinct();
        
        public static IEnumerable<string> FindVacant(DateTime dateTime)
        {
            var instituteIds = new List<string> {"25714", "25713"};
            var groupsIds = GroupIdFinder.FindInstituteGroupsId(instituteIds);
            var groupsSchedule = GetGroupsDaySchedules(groupsIds, dateTime);
            var lessons = GetLessonsAtTime(groupsSchedule, dateTime);
            return GetLessonsRooms(lessons);
        }
    }
}