using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using NCron;
using NCron.Fluent;
using NCron.Fluent.Crontab;
using NCron.Fluent.Generics;
using NCron.Service;
using Parser.ScheduleTasks;
using TelegramBot;


namespace MathMech_Everyday
{
    class Program
    {
        static void Main(string[] args)
        {
            var schedulingService = new SchedulingService();
            schedulingService.Daily().Run<ScheduleTasks>();

            var bot = new Bot(Secret.BotToken);
            bot.Start();
        }
    }
}