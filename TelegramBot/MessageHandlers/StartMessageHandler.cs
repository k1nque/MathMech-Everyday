using System.Collections.Generic;

namespace TelegramBot.MessageHandlers
{
    public class StartMessageHandler : MessageHandler
    {
        public StartMessageHandler()
        {
            Commands = new List<string>() {"/start"};
        }

        public override string GetMessage(long chatId)
        {
            return "Привет! Я пока могу делать следующие действия:" +
                   "\n/reg чтобы зарегистрироваться и быстро получать расписание" +
                   "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                   "(только для зарегистрированных пользователей)" +
                   "\n/help - расскажу про все мои возможности" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", " +
                   "чтобы получить актуальное расписание на сегодня";
        }
    }
}