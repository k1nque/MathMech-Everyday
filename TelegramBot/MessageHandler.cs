using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot
{
    public interface IMessageHandler
    {
        public Task<string> GetAnswerMessage(long chatId);
        public bool CheckRequestMessage(long chatId, string text);
    }
}