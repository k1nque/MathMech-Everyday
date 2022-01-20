using System.Threading.Tasks;

namespace TelegramBot.MessageHandlers
{
    public class OtherMessageHandler : IMessageHandler
    {
        public OtherMessageHandler()
        {
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            return true;
        }

        public async Task<string> GetAnswerMessage(long chatId)
        {
            return "Я пока не знаю такой команды, проверь правильно ли введены данные";
        }
    }
}