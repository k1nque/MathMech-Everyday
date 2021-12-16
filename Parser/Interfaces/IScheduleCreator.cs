using System;
using System.Threading.Tasks;

namespace Parser
{
    public interface IScheduleCreator
    {
        Task<Schedule> CreateScheduleById(string groupId, DateTime time);

        Task<Schedule> CreateScheduleByName(string groupName, DateTime time);
    }
}