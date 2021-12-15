using System;
using Parser;

namespace TelegramBot
{
    public class ScheduleMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }

        public string GetMessage(long chatId, string groupName)
        {
            var schedule = ScheduleCreator.CreateScheduleByName(groupName, DateTime.Now);
            return "Держи расписание, мне не жалко" +
                   "\n {schedule.ToString()}";
        }
    }
}