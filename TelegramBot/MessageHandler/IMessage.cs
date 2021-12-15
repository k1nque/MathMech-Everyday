using System;
using System.Linq;
using Parser;
using Telegram.Bot;

namespace TelegramBot
{
    public interface IMessageHandler
    {
        string[] textToCall { get; set; }
        string GetMessage(long chatId, string groupNumber = "");
        string commandDescription { get; set; }
    }
}