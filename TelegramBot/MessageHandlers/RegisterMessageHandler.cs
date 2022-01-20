using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class RegisterMessageHandler : IMessageHandler
    {
        private IUserState userState;

        public RegisterMessageHandler(IUserState userState)
        {
            this.userState = userState;
            // CommandDescription = "помогу зарегистрироваться и запомню тебя";
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            return (new List<string>() {"/reg"}).Contains(text.ToLower().Split()[0]);
        }

        public async Task<string> GetAnswerMessage(long chatId)
        {
            if (userState.GetChatStatus(chatId) == UserStatus.Registered)
            {
                return "Я всё видел, ты уже зарегистрировался! " +
                       "Теперь ты можешь воспользоваться функцией /ds";
            }

            userState.SetChatInfo(chatId, UserStatus.WaitingGroupName);
            return "Введи свой номер группы в формате \"МЕН-000000\" :)";
        }
    }
}