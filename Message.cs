using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MathMech_Everyday
{
    public class Message
    {
        private static Dictionary<long, Status> chatStatus = new Dictionary<long, Status>();

        private static Dictionary<long, string>
            registeredUsers = new Dictionary<long, string>(); //хранит номер группы зарегестрированных пользователей

        enum Status
        {
            newChat,
            waitingForName,
            waitingGroupNumber,
            registered
        };
        
        public static async Task GetMessage(TelegramBotClient bot)
        {
            
            var offset = 0;
            var timeout = 10;
            try
            {
                await bot.SetWebhookAsync("");
                while (true)
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout);
                    foreach (var update in updates)
                    {
                        await Message.MessageHandling(update, bot);
                        offset = update.Id + 1;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(String.Format("Error: {0}", exception));
            }
        }

        public static async Task MessageHandling(Telegram.Bot.Types.Update update, TelegramBotClient bot)
        {
            var message = update.Message;
            var text = message.Text.ToLower();
            var chatId = message.Chat.Id;
            if (!chatStatus.ContainsKey(chatId))
            {
                chatStatus.Add(chatId, Status.newChat);
            }

            //TODO.. switch/case
            if (text == "/start")
            {
                Console.WriteLine("Message received");
                await bot.SendTextMessageAsync(chatId, "Привет! Я пока могу делать следующие действия:" +
                                                       @"/n//reg чтобы зарегестрироваться" +
                                                       "/nПросто отправь номер группы, чтобы получить актуальное прямо сейчас расписание" +
                                                       "/n//help");
            }
            else if (text == "/reg")
            {
                if (chatStatus[chatId] == Status.registered)
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Я всё видел, ты уже зарегестрировался");
                }
                else
                {
                    //DataBaseController.Registration(message.Chat.Id.ToString(), message.Chat.Username.ToString());
                    await bot.SendTextMessageAsync(message.Chat.Id, "Введи своё ФИО :)");
                    chatStatus[message.Chat.Id] = Status.waitingForName;
                }
            }
            else if (text == "расписание")
            {
                if (chatStatus[chatId] == Status.registered)
                    await bot.SendTextMessageAsync(message.Chat.Id, "Держи расписание, мне не жалко");
                else
                    await bot.SendTextMessageAsync(message.Chat.Id, "Ты пока не зарегестрирован. " +
                                                                    "Введи номер своей группы если хочешь узнать расписание. " +
                                                                    "Или напиши /reg чтобы зарегестрироваться");
            }
            else
            {
                if (chatStatus.ContainsKey(message.Chat.Id))
                {
                    if (chatStatus[chatId] == Status.waitingForName)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Введи номер своей группы :)");
                        chatStatus[chatId] = Status.waitingGroupNumber;
                    }
                    else if (chatStatus[chatId] == Status.waitingGroupNumber)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id,
                            "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                            @"написав слово расписание ""расписание""");
                        chatStatus[chatId] = Status.registered;
                        registeredUsers.Add(chatId, text);
                    }
                }
            }
        }
    }
}