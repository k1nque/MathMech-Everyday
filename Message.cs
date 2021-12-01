using System;
using System.Collections.Generic;
using System.Linq;
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

            switch (text)
            {
                case "/start":
                    await bot.SendTextMessageAsync(chatId, "Привет! Я пока могу делать следующие действия:" +
                                                           "\n/reg чтобы зарегестрироваться и быстро получать расписание" +
                                                           "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                                                           "(только для зарегестрированных пользователей)" +
                                                           "\n/help - расскажу про все мои возможности" +
                                                           "\n/busy покажу какие кабинеты сейчас заняты" +
                                                           "\nИли росто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня");
                    break;

                case "/reg":
                    if (chatStatus[chatId] == Status.registered)
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Я всё видел, ты уже зарегестрировался!" +
                                                                        "Теперь ты можешь воспользоваться функцией /ds");
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id,
                            "Введи свой номер группы в формате \"МЕН-000000\" :)");
                        chatStatus[message.Chat.Id] = Status.waitingGroupNumber;
                    }

                    break;

                case ("/ds" or "расписание"):
                    if (chatStatus[chatId] == Status.registered)
                        await bot.SendTextMessageAsync(message.Chat.Id, "Держи расписание, мне не жалко");
                        //вызов метода Данила, который возвращает расписание на сегодня с registeredUsers[chatId]
                    else
                        await bot.SendTextMessageAsync(message.Chat.Id, "Ты пока не зарегестрирован. " +
                                                                        "Введи номер своей группы в формате \"МЕН-000000\"" +
                                                                        " если хочешь узнать расписание. " +
                                                                        "Или напиши /reg чтобы зарегестрироваться");
                    break;

                case "/busy":
                    var timeMessage = message.ForwardDate;
                    //показать кабинеты, которые сейчас заняты, вызов метода Данила с timeMessage
                    break;

                case "/help":
                    await bot.SendTextMessageAsync(message.Chat.Id, "\n/reg чтобы зарегестрироваться и быстро получать расписание" +
                                                                    "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                                                                    "(только для зарегестрированных пользователей)" +
                                                                    "\n/busy покажу какие кабинеты сейчас заняты" +
                                                                    "\nИли росто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня");
                    break;

                default:
                    if (chatStatus[chatId] == Status.waitingGroupNumber && Group.AllGroupNumbers.Contains(text))
                    {
                        //DataBaseController.Registration(message.Chat.Id.ToString(), message.Chat.Username.ToString());
                        await bot.SendTextMessageAsync(message.Chat.Id,
                            "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                            "написав слово \"расписание\" или вызвать команду /ds");
                        chatStatus[chatId] = Status.registered;
                        registeredUsers.Add(chatId, text);
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id,
                            "Я пока не знаю такой команды, проверь правильно ли введены данные");
                    }

                    //либо авторизация, либо ошибка с отправлением сообщения пользователю "я не знаю такую команду"
                    break;
                }
            }
        }
    }