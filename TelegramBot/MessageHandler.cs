using System.Collections.Generic;

namespace TelegramBot
{
    public abstract class MessageHandler
    {
        protected List<string> Commands = new List<string>();

        public abstract string GetMessage(long chatId);

        public virtual bool CheckMessage(long chatId, string text)
        {
            return Commands.Contains(text.ToLower().Split()[0]);
        }
    }
}