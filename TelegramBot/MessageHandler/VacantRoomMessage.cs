using System;
using System.Linq;
using Parser;

namespace TelegramBot
{
    public class VacantRoomMessage : IMessageHandler
    {
        public string[] textToCall { get; set; }
        public string commandDescription { get; set; }

        public VacantRoomMessage()
        {
            textToCall = new[] {"/busy"};
            commandDescription = "покажу какие кабинеты сейчас заняты";
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
}