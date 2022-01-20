using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class GroupNameMessageHandler : IMessageHandler
    {
        private IUserState userState;
        private IGroupIdFinder groupIdFinder;
        private string groupName;

        public GroupNameMessageHandler(IUserState userState, IGroupIdFinder groupIdFinder)
        {
            this.userState = userState;
            this.groupIdFinder = groupIdFinder;
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            var result = userState.GetChatStatus(chatId) == UserStatus.WaitingGroupName
                         && groupIdFinder.IsGroupName(text);
            if (result) groupName = text;
            return result;
        }

        public async Task<string> GetAnswerMessage(long chatId)
        {
            userState.SetChatInfo(chatId, UserStatus.Registered, groupName);
            return "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                   "написав слово \"расписание\" или вызвать команду /ds";
        }
    }
}