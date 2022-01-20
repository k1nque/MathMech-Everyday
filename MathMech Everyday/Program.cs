using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.Json;
using NCron.Fluent.Crontab;
using NCron.Fluent.Generics;
using NCron.Service;
using Parser.ScheduleTasks;
using TelegramBot;
using Ninject;
using Parser;
using TelegramBot.MessageHandlers;


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
            botConfig.ChatDatabaseFilename = Config.ChatDatabaseFilename;
            container.Bind<IUserState>().To<UserStateSQLite>();
            container.Bind<IGroupIdFinder>().To<GroupIdFinder>();
            container.Bind<IScheduleCreator>().To<ScheduleCreator>();
            container.Bind<IVacantRoomsFinder>().To<VacantRoomsFinder>();

            // bind msg handler
            /*var type = typeof(MessageHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var t in types)
            {
                container.Bind<IMessageHandler>().To<>
            }*/

            container.Bind<IMessageHandler>().To<StartMessageHandler>();
            container.Bind<IMessageHandler>().To<HelpMessageHandler>();
            container.Bind<IMessageHandler>().To<RegisterMessageHandler>();
            container.Bind<IMessageHandler>().To<ScheduleMessageHandler>();
            container.Bind<IMessageHandler>().To<RegisteredScheduleMessageHandler>();
            container.Bind<IMessageHandler>().To<GroupNameMessageHandler>();
            container.Bind<IMessageHandler>().To<VacantRoomMessageHandler>();
            container.Bind<IMessageHandler>().To<OtherMessageHandler>();

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