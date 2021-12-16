using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public enum UserStatus
    {
        NewChat,
        WaitingGroupNumber,
        Registered
    };
    
    public class UserState
    {
        

        // todo: переделать на запросы к базе данных

        private Dictionary<long, UserStatus> chatStatus = new Dictionary<long, UserStatus>();
        private Dictionary<long, string> chatGroupNumber = new Dictionary<long, string>();

        public UserStatus? GetChatStatus(long chatId)
        {
            return chatStatus.TryGetValue(chatId, out var result) ? result : null;
        }

        public void SetChatStatus(long chatId, UserStatus status)
        {
            chatStatus[chatId] = status;
        }

        public string GetChatGroupNumber(long chatId)
        {
            return chatGroupNumber.TryGetValue(chatId, out var result) ? result : null;
        }

        public void SetChatGroupNumber(long chatId, string groupNumber)
        {
            chatGroupNumber[chatId] = groupNumber;
        }
    }
}