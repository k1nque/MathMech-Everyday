using System;
using System.Data.SQLite;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MathMech_Everyday
{
    class Program
    {

        public static string token = "2104283130:AAH-kyJoKCZFT6crARvhLlZcum2lyhfRN3o";


        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var bot = new TelegramBotClient(token);
                    Message.GetMessage(bot).Wait();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(String.Format("Error: {0}", exception));
                }
            }
        }


    }
}