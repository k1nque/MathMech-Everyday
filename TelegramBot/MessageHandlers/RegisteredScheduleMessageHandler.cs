using System.Collections.Generic;
using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class RegisteredScheduleMessageHandler : MessageHandler
    {
        private IUserState userState;
        private ScheduleMessageHandler scheduleMessageHandler;

        public RegisteredScheduleMessageHandler(IUserState userState,
            IScheduleCreator scheduleCreator, IGroupIdFinder groupIdFinder)
        {
            this.userState = userState;
            this.scheduleMessageHandler = new ScheduleMessageHandler(scheduleCreator, groupIdFinder);
            Commands = new List<string>() {"/ds", "расписание", "р"};
            CommandDescription = "покажу твоё расписание (только для зарегистрированных пользователей)";
        }

        public override bool CheckMessage(long chatId, string text)
        {
            var tokens = text.Split();
            return tokens.Length == 1 && Commands.Contains(tokens[0].ToLower());
        }

        public override async Task<string> GetMessage(long chatId)
        {
            if (userState.GetChatStatus(chatId) == UserStatus.Registered)
            {
                scheduleMessageHandler.GroupName = userState.GetChatGroupName(chatId);
                return await scheduleMessageHandler.GetMessage(chatId);
            }

            userState.SetChatInfo(chatId, UserStatus.WaitingGroupName);
            return "Ты пока не зарегистрирован. " +
                   "Введи номер своей группы в формате \"МЕН-000000\"" +
                   " чтобы зарегистрироваться и узнать расписание";
        }
    }
}