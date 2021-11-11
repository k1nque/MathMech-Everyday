using System;
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
        private string botToken;
        private TelegramBotClient botClient;

        public Bot(string botToken)
        {
            this.botToken = botToken;
        }
        
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _                                       => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
    
            Console.WriteLine($"Received a '{update.Message.Text}' message in chat {chatId}.");

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   "You said:\n" + update.Message.Text
            );
        }
        
        

        public void Start()
        {
            this.botClient = new TelegramBotClient(this.botToken);
            botClient.StartReceiving(new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync));
            Console.ReadLine();
        }
    }
}