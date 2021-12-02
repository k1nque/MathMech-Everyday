using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public static class UserState
    {
        public enum Status
        {
            NewChat,
            WaitingForName,
            WaitingGroupNumber,
            Registered
        };

        // todo: переделать на запросы к базе данных

        private static Dictionary<long, Status> chatStatus = new Dictionary<long, Status>();
        private static Dictionary<long, string> chatGroupNumber = new Dictionary<long, string>();

        public static Status? GetChatStatus(long chatId)
        {
            return chatStatus.TryGetValue(chatId, out var result) ? result : null;
        }

        public static void SetChatStatus(long chatId, Status status)
        {
            chatStatus[chatId] = status;
        }

        public static string GetChatGroupNumber(long chatId)
        {
            return chatGroupNumber.TryGetValue(chatId, out var result) ? result : null;
        }

        public static void SetChatGroupNumber(long chatId, string groupNumber)
        {
            chatGroupNumber[chatId] = groupNumber;
        }
    }
}