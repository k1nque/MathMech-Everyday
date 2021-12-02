using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Text.Json;
using Parser;
using Telegram.Bot.Types;


namespace TelegramBot
{
    public static class MessageHandler
    {
        public static async Task PrintStart(TelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(chatId,
                "Привет! Я пока могу делать следующие действия:" +
                "\n/reg чтобы зарегестрироваться и быстро получать расписание" +
                "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                "(только для зарегистрированных пользователей)" +
                "\n/help - расскажу про все мои возможности" +
                "\n/busy покажу какие кабинеты сейчас заняты" +
                "\nИли просто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня");
        }

        public static async Task PrintHelp(TelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(chatId,
                "\n/reg чтобы зарегестрироваться и быстро получать расписание" +
                "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                "(только для зарегистрированных пользователей)" +
                "\n/busy покажу какие кабинеты сейчас заняты" +
                "\nИли просто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня");
        }

        public static async Task PrintSchedule(TelegramBotClient bot, long chatId)
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                await bot.SendTextMessageAsync(chatId, "Держи расписание, мне не жалко");

                //ScheduleCreator.CreateScheduleById(registeredUsers[chatId], DateTime.Now); //Если в листе хранятся id групп с сайта УрФУ 
                var schedule = ScheduleCreator.CreateScheduleByName(UserState.GetChatGroupNumber(chatId), DateTime.Now);
                await bot.SendTextMessageAsync(chatId, schedule.ToString());
            }
            else
            {
                await bot.SendTextMessageAsync(chatId,
                    "Ты пока не зарегистрирован. " +
                    "Введи номер своей группы в формате \"МЕН-000000\"" +
                    " если хочешь узнать расписание. " +
                    "Или напиши /reg чтобы зарегистрироваться");
            }
        }

        public static async Task PrintVacantRooms(TelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(chatId, "ъеъ");
        }

        public static async Task Register(TelegramBotClient bot, long chatId)
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                await bot.SendTextMessageAsync(chatId,
                    "Я всё видел, ты уже зарегистрировался!" +
                    "Теперь ты можешь воспользоваться функцией /ds");
            }
            else
            {
                await bot.SendTextMessageAsync(chatId,
                    "Введи свой номер группы в формате \"МЕН-000000\" :)");
                UserState.SetChatStatus(chatId, UserState.Status.WaitingGroupNumber);
            }
        }

        public static async Task SetGroupNumber(TelegramBotClient bot, long chatId, string groupNumber)
        {
            await bot.SendTextMessageAsync(chatId,
                "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                "написав слово \"расписание\" или вызвать команду /ds");
            UserState.SetChatStatus(chatId, UserState.Status.Registered);
            UserState.SetChatGroupNumber(chatId, groupNumber);
        }
    }
}