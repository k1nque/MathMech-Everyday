using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class StartMessageHandler : MessageHandler
    {
        private IUserState userState;
        
        public StartMessageHandler(IUserState userState)
        {
            this.userState = userState;
            Commands = new List<string>() {"/start"};
        }

        public override async Task<string> GetMessage(long chatId)
        {
            userState.SetChatInfo(chatId, UserStatus.NewChat);
            return "Привет! Я пока могу делать следующие действия:" +
                   "\n/reg чтобы зарегистрироваться и быстро получать расписание" +
                   "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня " +
                   "(только для зарегистрированных пользователей)" +
                   "\n/help - расскажу про все мои возможности" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", " +
                   "чтобы получить актуальное расписание на сегодня";
        }
    }
}