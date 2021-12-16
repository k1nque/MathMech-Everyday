using System;
using System.Net;
using System.Threading.Tasks;
using Ical.Net;
using static System.Globalization.CultureInfo;

namespace Parser
{
    public class CalendarCreator
    {
        private const string Url = "https://urfu.ru/api/schedule/groups/calendar/";

        public async Task<Calendar> GetCalendar(string id, DateTime creationTime)
        {
            var lastSundayDate = creationTime.GetLastSunday();
            var dt = lastSundayDate.ToString("yyyyMMdd", InvariantCulture);
            using var client = new WebClient();
            var content = await client.DownloadStringTaskAsync(Url + id + "/" + dt);
            return Calendar.Load(content);
        }
    }
}