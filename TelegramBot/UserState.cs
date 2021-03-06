using System;
using System.Data.SQLite;

namespace TelegramBot
{
    public enum UserStatus
    {
        NewChat,
        WaitingGroupNumber,
        Registered
    };

    public interface IUserState
    {
        public UserStatus? GetChatStatus(long chatId);
        public string GetChatGroupNumber(long chatId);
        public void SetChatInfo(long chatId, UserStatus status, string groupNumber = null);
        public void RemoveChat(long chatId);
    }

    public class UserState : IUserState
    {
        private SQLiteConnection connection;

        public UserState(string chatDatabaseFilename)
        {
            connection = new SQLiteConnection($"Data Source={chatDatabaseFilename}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "create table if not exists chats (" +
                              "chat_id integer primary key unique not null, " +
                              "status integer not null, group_number text)";
            cmd.ExecuteNonQuery();
        }

        ~UserState()
        {
            connection.Close();
        }

        public UserStatus? GetChatStatus(long chatId)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "select status from chats where chat_id == @chat_id limit 1";
            cmd.Parameters.AddWithValue("@chat_id", chatId);
            var result = cmd.ExecuteScalar();
            return result is null ? null : (UserStatus) Convert.ToInt32(result);
        }

        public string GetChatGroupNumber(long chatId)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "select group_number from chats where chat_id == @chat_id limit 1";
            cmd.Parameters.AddWithValue("@chat_id", chatId);
            return (string) cmd.ExecuteScalar();
        }

        public void SetChatInfo(long chatId, UserStatus status, string groupNumber = null)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "insert or replace into chats(chat_id, status, group_number) " +
                              "values (@chat_id, @status, @group_number)";
            cmd.Parameters.AddWithValue("@chat_id", chatId);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@group_number", groupNumber);
            cmd.ExecuteNonQuery();
        }

        public void RemoveChat(long chatId)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "delete from chats where chat_id == @chat_id";
            cmd.Parameters.AddWithValue("@chat_id", chatId);
            cmd.ExecuteNonQuery();
        }
    }
}