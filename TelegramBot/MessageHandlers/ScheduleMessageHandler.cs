using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class ScheduleMessageHandler : IMessageHandler
    {
        private IScheduleCreator scheduleCreator;
        private IGroupIdFinder groupIdFinder;
        public string GroupName { get; set; }

        public ScheduleMessageHandler(IScheduleCreator scheduleCreator, IGroupIdFinder groupIdFinder)
        {
            this.scheduleCreator = scheduleCreator;
            this.groupIdFinder = groupIdFinder;
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            var tokens = text.Split();
            var result = tokens.Length == 2 && 
                         (new List<string>() {"расписание", "р"}).Contains(tokens[0].ToLower()) 
                         && groupIdFinder.IsGroupName(tokens[1]);
            if (result) GroupName = tokens[1];
            return result;
        }

        public async Task<string> GetAnswerMessage(long chatId)
        {
            var schedule = await scheduleCreator.CreateScheduleByName(GroupName, DateTime.Now);
            return $"Держи расписание, мне не жалко\n{schedule.ToString()}";
        }
    }
}