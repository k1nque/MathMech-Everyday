using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class GetRequest
    {
        public static void DownloadCalendar()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://urfu.ru/api/schedule/groups/calendar/986697/20211107/", "calendar.ics");
            }
        }
    }
}
