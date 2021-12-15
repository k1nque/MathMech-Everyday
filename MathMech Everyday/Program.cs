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
using Ninject;


namespace MathMech_Everyday
{
    class Program
    {
        static void Main(string[] args)
        {
            var secrets = JsonSerializer.Deserialize<dynamic>("secrets.json");
            var container = new StandardKernel();

            container.Bind<string>()
                .ToConstant(secrets["BotToken"])
                .WhenInjectedInto<Bot>();

            container.Bind<Bot>()
                .ToSelf()
                .OnActivation(b => b.Start());

            container.Bind<SchedulingService>()
                .ToSelf()
                .OnActivation(s => s.Daily().Run<ScheduleTasks>());

            //var schedulingService = new SchedulingService();
            var schedulingService = container.Get<SchedulingService>();
            schedulingService.Daily().Run<ScheduleTasks>();
            //var bot = new Bot(Secrets["BotToken"]);
            var bot = container.Get<Bot>();
            //bot.Start();
        }
    }
}