using System;
using System.IO;
using System.Net;
using Ical.Net;
using static System.Globalization.CultureInfo;

namespace Parser
{
    public class CalendarCreator
    {
        private const string Url = "https://urfu.ru/api/schedule/groups/calendar/";
        private const string FileName = "calendar.ics";
        private void DownloadCalendar(string id, DateTime creationTime)
        {
            var lastSundayDate = creationTime.GetLastSunday();
            var dt = lastSundayDate.ToString("yyyyMMdd", InvariantCulture);
            using var client = new WebClient();
            client.DownloadFile(Url + id + "/" + dt, FileName);
        }

        private Calendar LoadCalendar()
        {
            var file = new StreamReader(FileName);
            var content = file.ReadToEnd();
            file.Close();
            File.Delete(FileName);
            return Calendar.Load(content);
        }

        public Calendar GetCalendar(string id, DateTime creationTime)
        {
            DownloadCalendar(id, creationTime);
            return LoadCalendar();
        }
    }
}