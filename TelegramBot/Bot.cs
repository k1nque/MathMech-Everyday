using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Parser;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.MessageHandlers;


namespace TelegramBot
{
    public class Bot
    {
        private TelegramBotClient botClient;
        private IUserState userState;
        private IGroupIdFinder groupIdFinder;
        private IScheduleCreator scheduleCreator;
        private IVacantRoomsFinder vacantRoomsFinder;
        private List<IMessageHandler> listOfPossibleMessageHandlers;

        public class Configuration
        {
            public string BotToken { get; set; }
            public string AllGroupsFilename { get; set; }
            public string MathMechGroupsFilename { get; set; }
            public string ChatDatabaseFilename { get; set; }
        }

        public Bot(Configuration config)
        {
            botClient = new TelegramBotClient(config.BotToken);
            userState = new UserStateSQLite(config.ChatDatabaseFilename);
            groupIdFinder = new GroupIdFinder(config.AllGroupsFilename, config.MathMechGroupsFilename);
            scheduleCreator = new ScheduleCreator(groupIdFinder);
            vacantRoomsFinder = new VacantRoomsFinder(scheduleCreator, groupIdFinder);
            listOfPossibleMessageHandlers = new List<IMessageHandler>()
            {
                new StartMessageHandler(userState),
                new HelpMessageHandler(),
                new RegisterMessageHandler(userState),
                new ScheduleMessageHandler(scheduleCreator, groupIdFinder),
                new RegisteredScheduleMessageHandler(userState, scheduleCreator, groupIdFinder),
                new GroupNameMessageHandler(userState, groupIdFinder),
                new VacantRoomMessageHandler(vacantRoomsFinder),
                new OtherMessageHandler()
            };
        }

        private Task HandleErrorAsync(ITelegramBotClient client, Exception exception,
            CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.Error.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message.Type != MessageType.Text
                                                  || string.IsNullOrEmpty(update.Message.Text))
                return;

            var text = update.Message.Text;
            var chatId = update.Message.Chat.Id;

            if (userState.GetChatStatus(chatId) == null)
            {
                userState.SetChatInfo(chatId, UserStatus.NewChat);
            }

            foreach (var messageHandler in listOfPossibleMessageHandlers.Where(
                         messageHandler => messageHandler.CheckRequestMessage(chatId, text)))
            {
                var answer = await messageHandler.GetAnswerMessage(chatId);
                await botClient.SendTextMessageAsync(chatId, answer, cancellationToken: cancellationToken);
                break;
            }
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions {AllowedUpdates = { }};

            //todo: переделать на вебхуки
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions,
                cancellationToken: cts.Token);
            Console.WriteLine("Start listening");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}