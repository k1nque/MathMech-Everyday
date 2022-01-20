using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parser;

namespace TelegramBot.MessageHandlers
{
    public class VacantRoomMessageHandler : IMessageHandler
    {
        private IVacantRoomsFinder vacantRoomsFinder;

        public VacantRoomMessageHandler(IVacantRoomsFinder vacantRoomsFinder)
        {
            this.vacantRoomsFinder = vacantRoomsFinder;
            // CommandDescription = "покажу какие кабинеты сейчас заняты";
        }

        public bool CheckRequestMessage(long chatId, string text)
        {
            return (new List<string>() {"/busy"}).Contains(text.ToLower().Split()[0]);
        }
        
        public async Task<string> GetAnswerMessage(long chatId)
        {
            var rooms = (await vacantRoomsFinder.FindVacant(DateTime.Now)).ToList();
            return rooms.Count > 0 ? $"Занятые аудитории: {string.Join(", ", rooms)}" : "Все аудитории свободны";
        }
    }
}