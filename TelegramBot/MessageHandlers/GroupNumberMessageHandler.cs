using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class GroupNumberMessageHandler : MessageHandler
    {
        private UserState userState;
        private IGroupIdFinder groupIdFinder;
        private string groupNumber;

        public GroupNumberMessageHandler(UserState userState, IGroupIdFinder groupIdFinder)
        {
            this.userState = userState;
            this.groupIdFinder = groupIdFinder;
        }

        public override bool CheckMessage(long chatId, string text)
        {
            var result = userState.GetChatStatus(chatId) == UserStatus.WaitingGroupNumber 
                         && groupIdFinder.IsGroupNumber(text);
            if (result) groupNumber = text;
            return result;
        }

        public override async Task<string> GetMessage(long chatId)
        {
            userState.SetChatStatus(chatId, UserStatus.Registered);
            userState.SetChatGroupNumber(chatId, groupNumber);
            return "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                   "написав слово \"расписание\" или вызвать команду /ds";
        }
    }
}