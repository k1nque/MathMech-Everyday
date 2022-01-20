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
        private ITelegramBotClient botClient;
        private readonly IUserState userState;
        private readonly IGroupIdFinder groupIdFinder;
        private readonly IScheduleCreator scheduleCreator;
        private readonly IVacantRoomsFinder vacantRoomsFinder;
        private readonly IEnumerable<IMessageHandler> listOfPossibleMessageHandlers;
        
        public Bot(
            //ITelegramBotClient botClient,
            IUserState userState,
            IGroupIdFinder groupIdFinder,
            IScheduleCreator scheduleCreator,
            IVacantRoomsFinder vacantRoomsFinder,
            IEnumerable<IMessageHandler> listOfPossibleMessageHandlers)
        {
            this.botClient = botClient;
            this.botClient = new TelegramBotClient("5074072293:AAFO0K4012TZb6A9rIEuDHe87gXUDDS1UGQ");
            this.userState = userState;
            this.groupIdFinder = groupIdFinder;
            this.scheduleCreator = scheduleCreator;
            this.vacantRoomsFinder = vacantRoomsFinder;
            this.listOfPossibleMessageHandlers = listOfPossibleMessageHandlers;
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