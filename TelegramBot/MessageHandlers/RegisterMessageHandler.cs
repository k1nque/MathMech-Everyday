using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class RegisterMessageHandler : MessageHandler
    {
        private IUserState userState;

        public RegisterMessageHandler(IUserState userState)
        {
            this.userState = userState;
            Commands = new List<string>() {"/reg"};
            CommandDescription = "помогу зарегистрироваться и запомню тебя";
        }

        public override async Task<string> GetMessage(long chatId)
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