using System;
using System.Data.SQLite;


namespace TelegramBot
{
    public class DataBaseController
    {
        public static SQLiteConnection DB;

        public static void Registration(string chatId, string username)
        {
            try
            {
                DB = new SQLiteConnection("Data Source=db.db;");
                DB.Open();
                SQLiteCommand regCMD = DB.CreateCommand();
                regCMD.CommandText = $"insert into RegUsers (ChatId, Username) values ('{chatId}', '{username}')";
                regCMD.ExecuteNonQuery();
                DB.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(String.Format("Error: {0}", exception));
            }
        }
    }
}