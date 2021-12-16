using System.Collections.Generic;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class RegisteredScheduleMessageHandler : MessageHandler
    {
        private UserState userState;
        private ScheduleMessageHandler scheduleMessageHandler;
        
        public RegisteredScheduleMessageHandler(UserState userState, 
            IScheduleCreator scheduleCreator, IGroupIdFinder groupIdFinder)
        {
            this.userState = userState;
            this.scheduleMessageHandler = new ScheduleMessageHandler(scheduleCreator, groupIdFinder);
            Commands = new List<string>() {"/ds", "расписание", "р"};
        }

        public override bool CheckMessage(long chatId, string text)
        {
            var tokens = text.Split();
            return tokens.Length == 1 && Commands.Contains(tokens[0].ToLower());
        }

        public override string GetMessage(long chatId)
        {
            if (userState.GetChatStatus(chatId) == UserStatus.Registered)
            {
                scheduleMessageHandler.GroupName = userState.GetChatGroupNumber(chatId);
                return scheduleMessageHandler.GetMessage(chatId);
            }

            userState.SetChatStatus(chatId, UserStatus.WaitingGroupNumber);
            return "Ты пока не зарегистрирован. " +
                   "Введи номер своей группы в формате \"МЕН-000000\"" +
                   " чтобы зарегистрироваться и узнать расписание";
        }
    }
}