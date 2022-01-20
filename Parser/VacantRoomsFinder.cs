using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parser
{
    public class VacantRoomsFinder: IVacantRoomsFinder
    {
        private readonly IScheduleCreator scheduleCreator;
        private readonly IGroupIdFinder groupIdFinder;

        public VacantRoomsFinder(IScheduleCreator creator, IGroupIdFinder finder)
        {
            scheduleCreator = creator;
            groupIdFinder = finder;
        }

        private async Task<IEnumerable<DaySchedule>> GetGroupsDaySchedules(IEnumerable<string> groupsId,
            DateTime dayToGet)
        {
            var schedulesTasks = groupsId
                .Select(id => scheduleCreator.CreateScheduleById(id, dayToGet));
            var schedules = await Task.WhenAll(schedulesTasks);
            var daySchedules = schedules
                .Select(schedule => schedule.GetDaySchedule(dayToGet.DayOfWeek));
            return daySchedules;
        }

        private static IEnumerable<Lesson> GetLessonsAtTime(
            IEnumerable<DaySchedule> groupsDaySchedule, DateTime timeToGet) =>
            groupsDaySchedule
                .SelectMany(daySchedule => daySchedule.Schedule
                    .Where(lesson => lesson.Location != null &&
                                     lesson.Start <= timeToGet && timeToGet <= lesson.End));

        private static IEnumerable<string> GetLessonsRooms(IEnumerable<Lesson> lessons) =>
            lessons.Select(lesson => lesson.Location).Distinct();

        public async Task<IEnumerable<string>> FindVacant(DateTime timeToFind)
        {
            var instituteIds = new List<string> {"25714", "25713"};
            var groupsIds = groupIdFinder.ExtractInstituteGroupsId(instituteIds);
            var groupsSchedule = await GetGroupsDaySchedules(groupsIds, timeToFind);
            var lessons = GetLessonsAtTime(groupsSchedule, timeToFind);
            return GetLessonsRooms(lessons);
        }
    }
}