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
            await Task.Yield(); //делает методы асинхронным
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
            foreach (var possibleMessage in listOfPossibleMessge)
            {
                foreach (var str in possibleMessage.textToCall)
                {
                    if (text == str)
                    {
                        var answer = possibleMessage.GetMessage(chatId);
                        await MessageHandler.Print(botClient, chatId, answer);
                        messageHasBeenSet = true;
                        break;
                    }
                }
                if (messageHasBeenSet) break;
            }
            if (!messageHasBeenSet) 
            {
                var answer = (new OtherMessage(update.Message.Text)).GetMessage(chatId);
                await MessageHandler.Print(botClient, chatId, answer);
            }
            
            //switch (text)
            //{
            //    //case "/start":
            //    //    await MessageHandler.PrintStart(botClient, chatId);
            //    //    break;
            //    //case "/help":
            //    //    await MessageHandler.PrintHelp(botClient, chatId);
            //    //    break;
            //    //case "/reg":
            //    //    await MessageHandler.Register(botClient, chatId);
            //    //    break;
            //    //case ("/ds" or "расписание"):
            //    //    await MessageHandler.PrintSchedule(botClient, chatId);
            //    //    break;
            //    //case "/busy":
            //    //    await MessageHandler.PrintVacantRooms(botClient, chatId);
            //    //    break;
            //    default:
            //        // todo: text лежит в lowercase, а в списке большими буквами
            //        if (UserState.GetChatStatus(chatId) == UserState.Status.WaitingGroupNumber
            //            && Group.AllGroupNumbers.Contains(update.Message.Text))
            //        {
            //            await MessageHandler.SetGroupNumber(botClient, chatId, update.Message.Text);
            //        }
            //        else if (text.Split().Length == 2 && text.Split()[0] == "р" &&
            //                 Group.AllGroupNumbers.Contains(update.Message.Text.Split()[1]))
            //        {
            //            await MessageHandler.PrintSchedule(botClient, chatId, update.Message.Text.Split()[1]);
            //        }
            //        else
            //        {
            //            await client.SendTextMessageAsync(chatId,
            //                "Я пока не знаю такой команды, проверь правильно ли введены данные");
            //        }

            //        break;
                
            //}
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