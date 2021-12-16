using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
        private static string GetBotToken()
        {
            if (!File.Exists(Config.SecretsFilename))
            {
                return null;
            }

            var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(Config.SecretsFilename));
            return secrets?["BotToken"];
        }

        static void Main(string[] args)
        {
            var botToken = GetBotToken();
            if (botToken is null)
            {
                Console.Error.WriteLine($"BotToken not found in {Config.SecretsFilename}");
                Environment.Exit(1);
            }

            var container = new StandardKernel();
            var botConfig = new Bot.Configuration();
            botConfig.BotToken = botToken;
            botConfig.AllGroupsFilename = Config.AllGroupsFilename;
            botConfig.MathMechGroupsFilename = Config.MathMechGroupsFilename;

            container.Bind<Bot.Configuration>()
                .ToConstant(botConfig)
                .WhenInjectedInto<Bot>();

            container.Bind<Bot>()
                .ToSelf()
                .OnActivation(b => b.Start());

            container.Bind<SchedulingService>()
                .ToSelf()
                .OnActivation(s => s.Daily().Run<ScheduleTasks>());

            var schedulingService = container.Get<SchedulingService>();
            schedulingService.Daily().Run<ScheduleTasks>();
            container.Get<Bot>();
        }
    }
}