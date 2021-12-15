using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramBot
{
    public class Bot
    {
        private TelegramBotClient botClient;
        internal List<IMessageHandler> listOfPossibleMessge = new List<IMessageHandler>() 
        {
            new StartMessage(),
            new HelpMessage(),
            new ScheduleMessage(),
            new ScheduleMessageRegistered(),
            new VacantRoomMessage(),
            new RegisterMessage(),
            new OtherMessage()
        };

        public Bot(string botToken)
        {
            this.botClient = new TelegramBotClient(botToken);
        }

        Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            await Task.Yield();
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var text = update.Message.Text.ToLower();
            var chatId = update.Message.Chat.Id;

            if (UserState.GetChatStatus(chatId) == null)
            {
                UserState.SetChatStatus(chatId, UserState.Status.NewChat);
            }
            
            var messageHasBeenSet = false;
            string answer = "";

            foreach (var possibleMessage in listOfPossibleMessge)
            {
                foreach (var str in possibleMessage.textToCall)
                {
                    if (text == str)
                    {
                        answer = possibleMessage.GetMessage(chatId);
                        messageHasBeenSet = true;
                        break;
                    }
                }
                if (messageHasBeenSet) break;
            }
            if (!messageHasBeenSet) 
            {
                answer = (new OtherMessage(update.Message.Text)).GetMessage(chatId);
            }
            await MessageHandler.Print(botClient, chatId, answer);
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