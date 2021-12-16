using System;
using System.Collections.Generic;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class ScheduleMessageHandler : MessageHandler
    {
        private IScheduleCreator scheduleCreator;
        public string GroupName { get; set; }

        public ScheduleMessageHandler(IScheduleCreator scheduleCreator)
        {
            this.scheduleCreator = scheduleCreator;
            Commands = new List<string>() {"расписание", "р"};
        }

        public override bool CheckMessage(long chatId, string text)
        {
            var tokens = text.Split();
            var result = tokens.Length == 2 && Commands.Contains(tokens[0].ToLower())
                                            && Groups.IsGroupNumber(tokens[1]);
            if (result) GroupName = tokens[1];
            return result;
        }

        public override string GetMessage(long chatId)
        {
            var schedule = scheduleCreator.CreateScheduleByName(GroupName, DateTime.Now);
            return $"Держи расписание, мне не жалко\n{schedule.ToString()}";
        }
    }
}