namespace TelegramBot
{
    public class ScheduleMessageRegistered : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }
        public ScheduleMessageRegistered()
        {
            textToCall = new[] { "/ds", "расписание" };
            commandDescription = "покажу твоё расписание (только для зарегестрированных пользователей)";
        }

        public string GetMessage(long chatId, string groupNumber = "")
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                return (new ScheduleMessage()).GetMessage(chatId, UserState.GetChatGroupNumber(chatId));
            }

            UserState.SetChatStatus(chatId, UserState.Status.WaitingGroupNumber);
            return "Ты пока не зарегистрирован. " +
                   "Введи номер своей группы в формате \"МЕН-000000\"" +
                   " чтобы зарегистрироваться и узнать расписание";
        }
    }
}