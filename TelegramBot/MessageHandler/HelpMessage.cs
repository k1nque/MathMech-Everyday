using System.Text;

namespace TelegramBot
{
    public class HelpMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }

        public HelpMessage()
        {
            textToCall = new[] { "/help" };
            commandDescription = "расскажу какие команды я знаю";
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            var builder = new StringBuilder();
            
            return "\n/reg пройдешь регистрацию и сможешь быстро получать расписание" +
                   "\n/ds или слово \"расписание\" покажу тебе твоё расписание на сегодня" +
                   "(только для зарегистрированных пользователей)" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня";
        }
    }
}