using System;
using System.Collections.Generic;
using System.IO;
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
        private const string SecretsFilename = "secrets.json";

        private static string GetBotToken()
        {
            if (!File.Exists(SecretsFilename))
            {
                return null;
            }

            var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(SecretsFilename));
            return secrets?["BotToken"];
        }

        static void Main(string[] args)
        {
            var botToken = GetBotToken();
            if (botToken is null)
            {
                Console.Error.WriteLine("BotToken not found in secrets.json");
                return;
            }

            var container = new StandardKernel();

            container.Bind<string>()
                .ToConstant(botToken)
                .WhenInjectedInto<Bot>();

            container.Bind<Bot>()
                .ToSelf()
                .OnActivation(b => b.Start());

            container.Bind<SchedulingService>()
                .ToSelf()
                .OnActivation(s => s.Daily().Run<ScheduleTasks>());

            var schedulingService = container.Get<SchedulingService>();
            schedulingService.Daily().Run<ScheduleTasks>();
            var bot = container.Get<Bot>();
        }
    }
}