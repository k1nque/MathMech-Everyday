using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Text.Json;
using Parser;
using Telegram.Bot.Types;


namespace TelegramBot
{
    public static class MessageHandler
    {
        public static async Task Print(TelegramBotClient bot, long chatId, string answer)
        {
            await bot.SendTextMessageAsync(chatId, answer);
        }
    }
}