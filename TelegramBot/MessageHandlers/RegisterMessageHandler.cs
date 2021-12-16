using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class RegisterMessageHandler : MessageHandler
    {
        private UserState userState;

        public RegisterMessageHandler(UserState userState)
        {
            this.userState = userState;
            Commands = new List<string>() {"/reg"};
        }

        public override async Task<string> GetMessage(long chatId)
        {
            if (userState.GetChatStatus(chatId) == UserStatus.Registered)
            {
                return "Я всё видел, ты уже зарегистрировался! " +
                       "Теперь ты можешь воспользоваться функцией /ds";
            }

            userState.SetChatStatus(chatId, UserStatus.WaitingGroupNumber);
            return "Введи свой номер группы в формате \"МЕН-000000\" :)";
        }
    }
}