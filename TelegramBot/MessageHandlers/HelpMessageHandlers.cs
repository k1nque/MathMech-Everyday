using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class HelpMessageHandler : MessageHandler
    {
        public HelpMessageHandler()
        {
            Commands = new List<string>() {"/help"};
            CommandDescription = "расскажу какие команды я знаю";
        }

        public override async Task<string> GetMessage(long chatId)
        {
            return "\n/reg чтобы зарегистрироваться и быстро получать расписание" +
                   "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня " +
                   "(только для зарегистрированных пользователей)" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", " +
                   "чтобы получить актуальное расписание на сегодня";
        }
    }
}