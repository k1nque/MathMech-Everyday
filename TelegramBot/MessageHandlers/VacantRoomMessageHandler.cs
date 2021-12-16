using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task<string> GetMessage(long chatId)
        {
            var rooms = (await vacantRoomsFinder.FindVacant(DateTime.Now)).ToList();
            return rooms.Count > 0 ? $"Занятые аудитории: {string.Join(", ", rooms)}" : "Все аудитории свободны";
        }
    }
}