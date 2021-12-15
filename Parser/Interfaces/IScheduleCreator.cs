using System;

namespace Parser
{
    public interface IScheduleCreator
    {
        Schedule CreateScheduleById(string groupId, DateTime time);

        Schedule CreateScheduleByName(string groupName, DateTime time);
    }
}