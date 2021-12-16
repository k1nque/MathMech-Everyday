using System;
using System.Collections.Generic;
using System.Linq;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class VacantRoomMessageHandler : MessageHandler
    {
        private VacantRoomsFinder vacantRoomsFinder;

        public VacantRoomMessageHandler(VacantRoomsFinder vacantRoomsFinder)
        {
            this.vacantRoomsFinder = vacantRoomsFinder;
            Commands = new List<string>() {"/busy"};
        }

        public override string GetMessage(long chatId)
        {
            var rooms = vacantRoomsFinder.FindVacant(DateTime.Now).ToList();
            if (rooms.Count > 0)
            {
                return $"Занятые аудитории: {string.Join(", ", rooms)}";
            }

            return "Все аудитории свободны";
        }
    }
}