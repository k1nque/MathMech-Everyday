namespace TelegramBot.MessageHandlers
{
    public class GroupNumberMessageHandler : MessageHandler
    {
        private UserState userState;
        private string groupNumber;

        public GroupNumberMessageHandler(UserState userState)
        {
            this.userState = userState;
        }

        public override bool CheckMessage(long chatId, string text)
        {
            var result = userState.GetChatStatus(chatId) == UserStatus.WaitingGroupNumber && Groups.IsGroupNumber(text);
            if (result) groupNumber = text;
            return result;
        }

        public override string GetMessage(long chatId)
        {
            userState.SetChatStatus(chatId, UserStatus.Registered);
            userState.SetChatGroupNumber(chatId, groupNumber);
            return "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                   "написав слово \"расписание\" или вызвать команду /ds";
        }
    }
}