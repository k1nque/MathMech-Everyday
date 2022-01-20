using System.Collections.Generic;
using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class RegisteredScheduleMessageHandler : IMessageHandler
    {
        private IUserState userState;
        private ScheduleMessageHandler scheduleMessageHandler;

        public RegisteredScheduleMessageHandler(IUserState userState,
            IScheduleCreator scheduleCreator, IGroupIdFinder groupIdFinder)
        {
            this.userState = userState;
            this.scheduleMessageHandler = new ScheduleMessageHandler(scheduleCreator, groupIdFinder);
            // CommandDescription = "покажу твоё расписание (только для зарегистрированных пользователей)";
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            var tokens = text.Split();
            return tokens.Length == 1 && (new List<string>() {"/ds", "расписание", "р"}).Contains(tokens[0].ToLower());
        }

        public async Task<string> GetAnswerMessage(long chatId)
        {
            if (userState.GetChatStatus(chatId) == UserStatus.Registered)
            {
                scheduleMessageHandler.GroupName = userState.GetChatGroupName(chatId);
                return await scheduleMessageHandler.GetAnswerMessage(chatId);
            }

            userState.SetChatInfo(chatId, UserStatus.WaitingGroupName);
            return "Ты пока не зарегистрирован. " +
                   "Введи номер своей группы в формате \"МЕН-000000\"" +
                   " чтобы зарегистрироваться и узнать расписание";
        }
    }
}