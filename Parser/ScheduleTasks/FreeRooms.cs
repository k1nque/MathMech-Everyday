using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NCron;

namespace Parser.ScheduleTasks
{
    public class ScheduleTasks : NCron.CronJob
    {
        public override void Execute()
        {
            // SaveCache(VacantRoomsFinder.FindVacant(DateTime.Now));
        }

        public void SaveCache(IEnumerable<string> cache)
        {
            var jsonString = JsonSerializer.Serialize(cache);
            File.WriteAllText(jsonString, "rooms.json");
        }
    }
}