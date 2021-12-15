using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
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
            var Secrets = JsonSerializer.Deserialize<dynamic>("Secret.json");
            var bot = new Bot(Secrets["BotToken"]);
            bot.Start();
        }
    }
}