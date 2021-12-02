using System;
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

            switch (text)
            {
                case "/start":
                    await MessageHandler.PrintStart(botClient, chatId);
                    break;
                case "/help":
                    await MessageHandler.PrintHelp(botClient, chatId);
                    break;
                case "/reg":
                    await MessageHandler.Register(botClient, chatId);
                    break;
                case ("/ds" or "расписание"):
                    await MessageHandler.PrintSchedule(botClient, chatId);
                    break;
                case "/busy":
                    await MessageHandler.PrintVacantRooms(botClient, chatId);
                    break;
                default:
                    // либо авторизация, либо ошибка с отправлением сообщения пользователю "я не знаю такую команду"
                    // todo: text лежит в lowercase, а в списке большими буквами
                    if (UserState.GetChatStatus(chatId) == UserState.Status.WaitingGroupNumber
                        && Group.AllGroupNumbers.Contains(update.Message.Text))
                    {
                        await MessageHandler.SetGroupNumber(botClient, chatId, update.Message.Text);
                    }
                    else
                    {
                        await client.SendTextMessageAsync(chatId,
                            "Я пока не знаю такой команды, проверь правильно ли введены данные");
                    }

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