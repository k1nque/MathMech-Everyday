using System;
using System.Linq;
using Parser;
using Telegram.Bot;

namespace TelegramBot
{
    public interface IMessageHandler
    {
        string[] textToCall { get; set; }
        string GetMessage(long chatId, string groupNumber = "");
    }

    public class StartMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }

        public StartMessage()
        {
            textToCall = new[] {"/start"};
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            return "Привет! Я пока могу делать следующие действия:" +
                   "\n/reg чтобы зарегистрироваться и быстро получать расписание" +
                   "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                   "(только для зарегистрированных пользователей)" +
                   "\n/help - расскажу про все мои возможности" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня";
        }
    }

    public class HelpMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }

        public HelpMessage()
        {
            textToCall = new[] {"/help"};
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            return "\n/reg чтобы зарегистрироваться и быстро получать расписание" +
                   "\n/ds или слово \"расписание\" - и я покажу тебе твоё расписание на сегодня" +
                   "(только для зарегистрированных пользователей)" +
                   "\n/busy покажу какие кабинеты сейчас заняты" +
                   "\nИли просто отправь номер группы в формате \"р МЕН-000000\", чтобы получить актуальное расписание на сегодня";
        }
    }

    public class ScheduleMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public ScheduleMessage()
        {
            textToCall = new[] { "" };
        }
        public string GetMessage(long chatId, string groupName)
        {
            var schedule = ScheduleCreator.CreateScheduleByName(groupName, DateTime.Now);
            return "Держи расписание, мне не жалко" +
                   "\n {schedule.ToString()}";
        }
    }

    public class ScheduleMessageRegistered : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public ScheduleMessageRegistered()
        {
            textToCall = new[] { "/ds", "расписание" };
        }

        public string GetMessage(long chatId, string groupNumber = "")
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                return (new ScheduleMessage()).GetMessage(chatId, UserState.GetChatGroupNumber(chatId));
            }

            UserState.SetChatStatus(chatId, UserState.Status.WaitingGroupNumber);
            return "Ты пока не зарегистрирован. " +
                   "Введи номер своей группы в формате \"МЕН-000000\"" +
                   " чтобы зарегистрироваться и узнать расписание";
        }
    }
    
    public class VacantRoomMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }

        public VacantRoomMessage()
        {
            textToCall = new[] { "/busy" };
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            var rooms = VacantRoomsFinder.FindVacant(DateTime.Now).ToList();
            if (rooms.Count > 0)
            {
                return $"Занятые аудитории: {string.Join(", ", rooms)}";    
            }
            return "Все аудитории свободны";
        }
    }
    
    public class RegisterMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public RegisterMessage()
        {
            textToCall = new[] { "/reg" };
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            if (UserState.GetChatStatus(chatId) == UserState.Status.Registered)
            {
                return "Я всё видел, ты уже зарегистрировался! " +
                       "Теперь ты можешь воспользоваться функцией /ds";
            }
            UserState.SetChatStatus(chatId, UserState.Status.WaitingGroupNumber);
            return "Введи свой номер группы в формате \"МЕН-000000\" :)";
        }
    }
    
    public class OtherMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        private string text;

        public OtherMessage(string text)
        {
            this.text = text;
        }

        public OtherMessage()
        {
            textToCall = new[] {""};
        }
        public string GetMessage(long chatId, string groupNumber = "")
        {
            text.ToUpper();
            var status = UserState.GetChatStatus(chatId);
            var check = Group.AllGroupNumbers.Contains(text);
            if (status == UserState.Status.WaitingGroupNumber
                        && check)
            {
                return SetGroupNumber(chatId, text);
            }
            else if (text.Split().Length == 2 && text.Split()[0] == "р" &&
                     Group.AllGroupNumbers.Contains(text.Split()[1]))
            {
                return (new ScheduleMessage()).GetMessage(chatId, text.Split()[1]);
            }
            else
            {
                return "Я пока не знаю такой команды, проверь правильно ли введены данные";
            }
        }

        public static string SetGroupNumber(long chatId, string groupNumber)
        {
            UserState.SetChatStatus(chatId, UserState.Status.Registered);
            UserState.SetChatGroupNumber(chatId, groupNumber);
            return "Прекрасно, теперь ты можешь получать расписание своей группы просто" +
                "написав слово \"расписание\" или вызвать команду /ds";
        }
    }
}