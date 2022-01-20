﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBot
{
    public interface IMessageHandler
    {
        public Task<string> GetAnswerMessage(long chatId);
        public bool CheckRequestMessage(long chatId, string text);
    }
    public abstract class MessageHandler
    {
        protected List<string> Commands = new List<string>();
        public string CommandDescription { get; set; }

        public abstract Task<string> GetMessage(long chatId);

        public virtual bool CheckMessage(long chatId, string text)
        {
            return Commands.Contains(text.ToLower().Split()[0]);
        }
    }
}