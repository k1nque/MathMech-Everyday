using System;
using System.Collections.Generic;
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
        private UserState userState;
        private IGroupIdFinder groupIdFinder;
        private IScheduleCreator scheduleCreator;
        private VacantRoomsFinder vacantRoomsFinder;
        private List<MessageHandler> listOfPossibleMessageHandlers;

        public Bot(string botToken)
        {
            botClient = new TelegramBotClient(botToken);
            userState = new UserState();
            groupIdFinder = new GroupIdFinder();
            scheduleCreator = new ScheduleCreator(groupIdFinder);
            vacantRoomsFinder = new VacantRoomsFinder(scheduleCreator, groupIdFinder);
            listOfPossibleMessageHandlers = new List<MessageHandler>()
            {
                new StartMessageHandler(),
                new HelpMessageHandler(),
                new RegisterMessageHandler(userState),
                new ScheduleMessageHandler(scheduleCreator),
                new RegisteredScheduleMessageHandler(userState, scheduleCreator),
                new GroupNumberMessageHandler(userState),
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
                userState.SetChatStatus(chatId, UserStatus.NewChat);
            }

            foreach (var messageHandler in listOfPossibleMessageHandlers)
            {
                if (messageHandler.CheckMessage(chatId, text))
                {
                    var answer = messageHandler.GetMessage(chatId);
                    await botClient.SendTextMessageAsync(chatId, answer, cancellationToken: cancellationToken);
                    break;
                }
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