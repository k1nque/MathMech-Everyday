using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class OtherMessageHandler : MessageHandler
    {
        public OtherMessageHandler()
        {
        }

        public override bool CheckMessage(long chatId, string text)
        {
            return true;
        }

        public override async Task<string> GetMessage(long chatId)
        {
            return "Я пока не знаю такой команды, проверь правильно ли введены данные";
        }
    }
}